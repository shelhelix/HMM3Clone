using System;
using System.Collections.Generic;
using Hmm3Clone.Config;
using Hmm3Clone.Gameplay;
using Hmm3Clone.State;
using Hmm3Clone.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hmm3Clone.Controller {
	public class CityController : IController {
		public const string TestCityName = "TestCity";

		const int InvalidIndex = -1;

		readonly MapState _mapState;

		readonly UnitsController _unitsController;
		
		readonly BuildingConfig _buildingConfig;
		
		readonly ResourceController _resourceController;

		public event Action OnGarrisonChanged;

		public event Action OnBuildingsChanged;
		

		public CityController(ResourceController resourceController, UnitsController unitsController, TurnController turnController, MapState mapState) {
			_mapState = mapState;

			// only for testing
			if (!_mapState.Cities.Exists(x => x.CityName == TestCityName)) {
				var city = CreateCityState(TestCityName);
				_mapState.Cities.Add(city);
			}

			_resourceController = resourceController;
			_buildingConfig     = ConfigLoader.LoadConfig<BuildingConfig>();
			_unitsController    = unitsController;

			turnController.OnTurnChanged += OnTurnChanged;
		}

		public Army GetCityGarrison(string cityName) {
			return new Army(GetCityState(cityName).Garrison);
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

		public void SplitStacks(string cityName, int sourceStackIndex, int dstStackIndex) {
			var army = GetCityGarrison(cityName);
			army.SplitStack(sourceStackIndex, dstStackIndex);
			OnGarrisonChanged?.Invoke();
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
			var state     = GetCityState(cityName);
			return FindStackInGarrison(state, unitType) != null || (GetFreeStackInGarrison(state) != InvalidIndex);
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

		int GetFreeStackInGarrison(CityState state) {
			for (var i = 0; i < state.Garrison.Length; i++) {
				if (state.Garrison[i] == null) {
					return i;
				}
			}
			return -1;
		}
		
		int GetUnitStackInGarrisonIndex(CityState state, UnitStack unitStack) {
			for (var i = 0; i < state.Garrison.Length; i++) {
				if (state.Garrison[i] == unitStack) {
					return i;
				}
			}
			return -1;
		}

		UnitStack FindStackInGarrison(CityState city, UnitType type) {
			foreach (var unitStack in city.Garrison) {
				if (unitStack != null && unitStack.Type == type) {
					return unitStack;
				}
			}
			return null;
		}

		bool HasStackInGarrison(CityState state, UnitStack stack) {
			foreach (var unitStack in state.Garrison) {
				if (unitStack == stack) {
					return true;
				}
			}
			return false;
		}

		UnitStack GetOrCreateUnitStack(CityState state, UnitType unitType) {
			var unitStack = FindStackInGarrison(state, unitType);
			if (unitStack != null) {
				return unitStack;
			}
			var freeStackIndex = GetFreeStackInGarrison(state);
			if (freeStackIndex == InvalidIndex) {
				return null;
			}
			unitStack                      = new UnitStack {Type = unitType};
			state.Garrison[freeStackIndex] = unitStack;
			return unitStack;
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
			var unitStack = GetOrCreateUnitStack(cityState, unitType);
			Assert.IsNotNull(unitStack);
			cityState.ReadyToBuyUnits.IncrementAmount(baseUnitForm, -count);
			unitStack.Amount += count;
			OnGarrisonChanged?.Invoke();
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

		public void TransformStacks(string cityName, int movingStackIndex, int stableStackIndex) {
			if (movingStackIndex == stableStackIndex) {
				return;
			}
			Assert.IsTrue(movingStackIndex >= 0 && stableStackIndex >= 0);
			var garrison    = GetCityGarrison(cityName);
			if (garrison.AreMergeableStacks(movingStackIndex, stableStackIndex)) {
				garrison.MergeStack(movingStackIndex, stableStackIndex);
			} else {
				garrison.SwapStack(movingStackIndex, stableStackIndex);	
			}
			OnGarrisonChanged?.Invoke();
		}
	}
}