using System;
using System.Collections.Generic;
using Hmm3Clone.Behaviour;
using Hmm3Clone.Config;
using Hmm3Clone.Gameplay;
using Hmm3Clone.State;
using Hmm3Clone.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hmm3Clone.Controller {
	public class CityController : IController {
		public const string TestCityName = "TestCity";


		readonly MapState _mapState;

		readonly UnitsController    _unitsController;
		readonly ResourceController _resourceController;
		readonly HeroController     _heroController;
		
		readonly BuildingConfig _buildingConfig;
		

		public event Action OnArmyChanged;
		public event Action OnBuildingsChanged;
		

		public CityController(ResourceController resourceController, UnitsController unitsController, HeroController heroController, TurnController turnController, MapState mapState) {
			_mapState = mapState;

			// TODO: remove after testing. This code is only for testing 
			if (!_mapState.Cities.Exists(x => x.CityName == TestCityName)) {
				var city = CreateCityState(TestCityName);
				city.GuestHero = HeroController.TestHeroName;
				_mapState.Cities.Add(city);
			}

			_heroController     = heroController;
			_resourceController = resourceController;
			_buildingConfig     = ConfigLoader.LoadConfig<BuildingConfig>();
			_unitsController    = unitsController;

			turnController.OnTurnChanged += OnTurnChanged;
		}

		public Army GetCityGarrison(string cityName) {
			var state = GetCityState(cityName);
			if (string.IsNullOrEmpty(state.HeroInGarrison)) {
				return new Army(GetCityState(cityName).Garrison);
			}
			var hero = _heroController.GetHero(state.HeroInGarrison);
			Assert.IsNotNull(hero, $"Can't find hero with name {state.HeroInGarrison}");
			return hero.Army;
		}

		public CityState GetCityState(string cityName) {
			var state = _mapState.Cities.Find(x => x.CityName == cityName);
			Assert.IsNotNull(state);
			return state;
		}

		public List<Resource> GetUnitHiringPrice(UnitType unitType, int amount = 1) {
			var oneUnitPrice = _unitsController.GetUnitInfo(unitType).HirePrice;
			var res          = new List<Resource>();
			oneUnitPrice.ForEach(x => res.Add(new Resource(x.ResourceType, x.Amount * amount)));
			return res;
		}

		public bool TrySplitStacks(string cityName, CityUnitStackIndex sourceIndex, CityUnitStackIndex destIndex) {
			if (sourceIndex.Equals(destIndex)) {
				return true;
			}
			
			var sourceArmy = GetArmy(cityName, sourceIndex.ArmySource);
			var destArmy   = GetArmy(cityName, destIndex.ArmySource);
			Assert.IsNotNull(sourceArmy, $"Source army from {sourceIndex.ArmySource} {sourceIndex.StackIndex} is null");
			if (destArmy == null) {
				return false;
			}
			if (!destArmy.IsStackEmpty(destIndex.StackIndex)) {
				Debug.LogWarning("Can't split units between two used stacks yet");
				return false;
			}
			sourceArmy.SplitStack(sourceIndex.StackIndex, destArmy, destIndex.StackIndex);
			OnArmyChanged?.Invoke();
			return true;
		}
		
		public Dictionary<ResourceType, int> GetCityIncome(string cityName) {
			return GetCityIncome(GetCityState(cityName));
		}

		public Dictionary<UnitType, int> GetNotBoughtCityUnits(string cityName) {
			return GetCityState(cityName).ReadyToBuyUnits;
		}

		public int GetMaxAvailableToBuyUnitsAmount(string cityName, UnitType unitType) {
			var unitsInCity = GetReadyToBuyUnitsAmount(cityName, unitType);
			var hirePrice   = GetUnitHiringPrice(unitType);

			var unitsMinAmountByResources = int.MaxValue;
			foreach (var resourcePrice in hirePrice) {
				var currentResourceAmount = _resourceController.GetResourceAmount(resourcePrice);
				var maxAmount             = currentResourceAmount / resourcePrice.Amount;
				unitsMinAmountByResources = Mathf.Min(maxAmount, unitsMinAmountByResources);
			}

			return Mathf.Min(unitsInCity, unitsMinAmountByResources);
		}

		public bool HasAvailableStackForUnits(string cityName, UnitType unitType) {
			var garrison = GetCityGarrison(cityName);
			return garrison.FindStackWithUnits(unitType) != null || garrison.GetFreeStackIndex() != Army.InvalidStackIndex;
		}

		public bool IsErected(string cityName, BuildingType buildingType) {
			var cityState = GetCityState(cityName);
			return cityState.IsErected(buildingType);
		}
		
		public bool CanErectBuilding(string cityName, BuildingType buildingType) {
			var cityState    = GetCityState(cityName);
			var buildingInfo = GetBuildingInfo(buildingType);
			return buildingInfo.BuildingCost.TrueForAll(_resourceController.IsEnoughResource)
				   && buildingInfo.Dependencies.TrueForAll(x => cityState.IsErected(x.Name))
				   && cityState.CanErectBuilding;
		}
		
		public void ErectBuilding(string cityName, BuildingType buildingType) {
			var cityState = GetCityState(cityName);
			if (!CanErectBuilding(cityName, buildingType)) {
				return;
			}

			var buildingInfo = GetBuildingInfo(buildingType);
			foreach (var res in buildingInfo.BuildingCost) {
				_resourceController.SubResources(res);
			}
			
			cityState.ErectBuilding(buildingType);
			cityState.CanErectBuilding = false;
			OnBuildingsChanged?.Invoke();
		}

		BuildingInfo GetBuildingInfo(BuildingType buildingType) {
			return _buildingConfig.Buildings.Find(x => x.Name == buildingType);
		}

		public bool CanHireUnit(string cityName, UnitType unitType) {
			var state = GetCityState(cityName);
			foreach (var building in state.ErectedBuildings) {
				var unitUpgradeInfo    = _buildingConfig.GetUnitsUpgradeInfo(building);
				if (unitUpgradeInfo.Contains(unitType)) {
					return true;
				}
				var unitProductionInfo = _buildingConfig.GetUnitProductionInfo(building);
				if (unitProductionInfo.Exists(x => x.UnitType == unitType)) {
					return true;
				}
			}
			return false;
		}
		
		public void HireUnits(string cityName, UnitType unitType, int count) {
			Assert.IsTrue(HasAvailableStackForUnits(cityName, unitType));
			var cityState    = GetCityState(cityName);
			var baseUnitForm = _unitsController.GetBaseUnitType(unitType);
			Assert.IsTrue(cityState.ReadyToBuyUnits.GetOrDefault(baseUnitForm) >= count);
			var price = GetUnitHiringPrice(unitType, count);
			Assert.IsTrue(price.TrueForAll(_resourceController.IsEnoughResource));
			price.ForEach(_resourceController.SubResources);
			// hire units
			var garrison  = GetCityGarrison(cityName);
			var unitStack = garrison.GetOrCreateUnitStack(unitType);
			Assert.IsNotNull(unitStack);
			cityState.ReadyToBuyUnits.IncrementAmount(baseUnitForm, -count);
			unitStack.Amount += count;
			OnArmyChanged?.Invoke();
		}
		
		Dictionary<UnitType, int> GetUnitProductionAmount(string cityName) {
			var state = GetCityState(cityName);
			var res = new Dictionary<UnitType, int>();
			foreach (var buildingName in state.ErectedBuildings) {
				var productionInfo = _buildingConfig.GetUnitProductionInfo(buildingName);
				productionInfo?.ForEach(x => res.IncrementAmount(x.UnitType, x.Amount));
			}
			return res;
		}

		void OnTurnChanged(int currentTurn) {
			ProduceResources();
			if (IsFirstDayOfTheWeek(currentTurn)) {
				ProduceUnits();
			}
			_mapState.Cities.ForEach(x => x.CanErectBuilding = true);	
 		}

		bool IsFirstDayOfTheWeek(int turnCount) {
			return turnCount % 7 == 0;
		}
		
		void ProduceUnits() {
			foreach (var cityState in _mapState.Cities) {
				var unitProduction = GetUnitProductionAmount(cityState.CityName);
				foreach (var production in unitProduction) {
					cityState.ReadyToBuyUnits.IncrementAmount(production.Key, production.Value);
				}
			}
		}

		void ProduceResources() {
			foreach (var cityState in _mapState.Cities) {
				foreach (var income in GetCityIncome(cityState.CityName)) {
					_resourceController.AddResource(new Resource(income.Key, income.Value));
				}
			}
		}

		CityState CreateCityState(string cityName) {
			var state = new CityState(cityName);
			state.ErectBuilding(BuildingType.TownHall);
			return state;
		}

		Dictionary<ResourceType, int> GetCityIncome(CityState state) {
			Assert.IsNotNull(state);
			var accumulatedCityProduction = new Dictionary<ResourceType, int>();
			foreach (var buildingName in state.ErectedBuildings) {
				var resourcesInfo = _buildingConfig.GetResourcesProductionInfo(buildingName);
				resourcesInfo?.ForEach(x => accumulatedCityProduction.IncrementAmount(x.ResourceType, x.Amount));
			}
			return accumulatedCityProduction;
		}

		int GetReadyToBuyUnitsAmount(string cityName, UnitType unitType) {
			var cityState    = GetCityState(cityName);
			// "ready to buy" is common amount for base and advanced form of units
			var baseUnitForm = _unitsController.GetBaseUnitType(unitType);
			return cityState.ReadyToBuyUnits.GetOrDefault(baseUnitForm);
		}

		public void TransformStacks(string cityName, CityUnitStackIndex sourceStackIndex, CityUnitStackIndex destStackIndex) {
			if (sourceStackIndex.Equals(destStackIndex)) {
				return;
			}

			var oneArmy   = GetArmy(cityName, sourceStackIndex.ArmySource);
			var otherArmy = GetArmy(cityName, destStackIndex.ArmySource);

			if (otherArmy == null) {
				return;
			}
			
			if (oneArmy.AreMergeableStacks(sourceStackIndex.StackIndex, otherArmy, destStackIndex.StackIndex)) {
				oneArmy.MergeStack(sourceStackIndex.StackIndex, otherArmy, destStackIndex.StackIndex);
			} else {
				oneArmy.SwapStacks(sourceStackIndex.StackIndex, otherArmy, destStackIndex.StackIndex);	
			}
			OnArmyChanged?.Invoke();
		}

		public string GetGuestHeroName(string cityName) {
			var cityState = GetCityState(cityName);
			return cityState.GuestHero;
		}

		Army GetArmy(string cityName, ArmySource source) {
			return (source == ArmySource.Garrison)
					   ? GetCityGarrison(cityName)
					   : GetGuestHeroArmy(cityName);
		}

		Army GetGuestHeroArmy(string cityName) {
			var guestHeroName = GetCityState(cityName).GuestHero;
			return string.IsNullOrEmpty(guestHeroName) ? null : _heroController.GetHero(guestHeroName).Army;
		}

		public void TrySwapHeroesInCity(string cityName, ArmySource source, ArmySource dest) {
			var state = GetCityState(cityName);
			if (source == ArmySource.GuestHero && dest == ArmySource.Garrison && string.IsNullOrEmpty(state.HeroInGarrison)) {
				MergeGuestHeroAndGarrisonArmies(cityName);
			} else {
				SwapHeroesInCity(cityName);
			}
			OnArmyChanged?.Invoke();
		}

		void SwapHeroesInCity(string cityName) {
			var state = GetCityState(cityName);
			(state.GuestHero, state.HeroInGarrison) = (state.HeroInGarrison, state.GuestHero);
			OnArmyChanged?.Invoke();
		}

		void MergeGuestHeroAndGarrisonArmies(string cityName) {
			var guestHeroArmy = GetArmy(cityName, ArmySource.GuestHero);
			var garrisonArmy  = GetArmy(cityName, ArmySource.Garrison);
			Assert.IsNotNull(guestHeroArmy);
			Assert.IsNotNull(garrisonArmy);
			if (!garrisonArmy.TryMergeWithOtherArmy(garrisonArmy)) {
				return;
			}
			SwapHeroesInCity(cityName);
		}

		public string GetGarrisonHeroName(string cityName) {
			return GetCityState(cityName)?.HeroInGarrison;
		}
	}
}