using System;
using System.Collections.Generic;
using Hmm3Clone.Config;
using Hmm3Clone.State;
using Hmm3Clone.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hmm3Clone.Controller {
	public class CityController : IController {
		public const string TestCityName = "TestCity";

		readonly MapState _mapState;

		readonly BuildingsProductionConfig _productionConfig;
		readonly UnitInfoConfig            _unitInfoConfig;
		
		
		readonly ResourceController _resourceController;

		public event Action OnGarrisonChanged;
		

		public CityController(ResourceController resourceController, TurnController turnController, MapState mapState) {
			_mapState = mapState;

			// only for testing
			if (!_mapState.Cities.Exists(x => x.CityName == TestCityName)) {
				var city = CreateCityState(TestCityName);
				_mapState.Cities.Add(city);
			}

			_resourceController = resourceController;
			_productionConfig   = ConfigLoader.LoadConfig<BuildingsProductionConfig>();
			_unitInfoConfig     = ConfigLoader.LoadConfig<UnitInfoConfig>();

			turnController.OnTurnChanged += OnTurnChanged;
		}

		public List<UnitStack> GetCityGarrison(string cityName) {
			return GetCityState(cityName).Garrison;
		}

		public CityState GetCityState(string cityName) {
			var state = _mapState.Cities.Find(x => x.CityName == cityName);
			Assert.IsNotNull(state);
			return state;
		}

		public List<Resource> GetUnitHiringPrice(UnitType unitType, int amount = 1) {
			var oneUnitPrice = _unitInfoConfig.GetUnitInfo(unitType).HirePrice;
			var res          = new List<Resource>();
			oneUnitPrice.ForEach(x => res.Add(new Resource(x.ResourceType, x.Amount * amount)));
			return res;
		}
		
		public Dictionary<ResourceType, int> GetCityIncome(string cityName) {
			return GetCityIncome(GetCityState(cityName));
		}

		public Dictionary<UnitType, int> GetNotBoughtCityUnits(string cityName) {
			return GetCityState(cityName).ReadyToBuyUnits;
		}
		
		public UnitStack GetOrCreateGarrisonUnitStack(string cityName, UnitType unitType) {
			var state = GetCityState(cityName);
			return state.GetOrCreateUnitStack(unitType);
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

		public bool HasStackForUnits(string cityName, UnitType unitType) {
			var state     = GetCityState(cityName);
			var unitStack = state.GetUnitStack(unitType);
			return unitStack != null || state.Garrison.Count < CityState.MaxUnitStacksCount;
		}  

		public void HireUnits(string cityName, UnitType unitType, int count) {
			Assert.IsTrue(HasStackForUnits(cityName, unitType));
			var cityState = GetCityState(cityName);
			Assert.IsTrue(cityState.ReadyToBuyUnits.GetOrDefault(unitType) >= count);
			var price = GetUnitHiringPrice(unitType, count);
			Assert.IsTrue(price.TrueForAll(_resourceController.IsEnoughResource));
			price.ForEach(_resourceController.SubResources);
			// hire units
			var unitStack = cityState.GetOrCreateUnitStack(unitType);
			Assert.IsNotNull(unitStack);
			cityState.ReadyToBuyUnits.IncrementAmount(unitType, -count);
			unitStack.Amount += count;
			OnGarrisonChanged?.Invoke();
		}
		
		Dictionary<UnitType, int> GetUnitProductionAmount(string cityName) {
			var state = GetCityState(cityName);
			var res = new Dictionary<UnitType, int>();
			foreach (var buildingName in state.ErectedBuildings) {
				var productionInfo = _productionConfig.GetUnitProductionInfo(buildingName);
				if (productionInfo != null) {
					res.IncrementAmount(productionInfo.UnitType, productionInfo.Amount);
				}
			}
			return res;
		}

		void OnTurnChanged(int currentTurn) {
			ProduceResources();
			if (IsFirstDayOfTheWeek(currentTurn)) {
				ProduceUnits();
			}
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
				var productionInfo = _productionConfig.GetProductionInfo(buildingName);
				productionInfo?.ResourcesProduction.ForEach(x => {
					accumulatedCityProduction.IncrementAmount(x.ResourceType, x.Amount);
				});
			}
			return accumulatedCityProduction;
		}


		int GetReadyToBuyUnitsAmount(string cityName, UnitType unitType) {
			var cityState = GetCityState(cityName);
			return cityState.ReadyToBuyUnits.GetOrDefault(unitType);
		}
	}
}