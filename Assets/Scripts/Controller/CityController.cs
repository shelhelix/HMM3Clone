using System.Collections.Generic;
using Hmm3Clone.Config;
using Hmm3Clone.State;
using Hmm3Clone.Utils;
using TMPro;
using UnityEngine.Assertions;

namespace Hmm3Clone.Controller {
	public class CityController : IController {
		public const string TestCityName = "TestCity";

		readonly MapState _mapState;

		readonly BuildingsProductionConfig _productionConfig;

		readonly ResourceController _resourceController;


		public CityController(ResourceController resourceController, TurnController turnController, MapState mapState) {
			_mapState = mapState;

			// only for testing
			if (!_mapState.Cities.Exists(x => x.CityName == TestCityName)) {
				var city = CreateCityState(TestCityName);
				_mapState.Cities.Add(city);
			}

			_resourceController = resourceController;
			_productionConfig = ConfigLoader.LoadConfig<BuildingsProductionConfig>();

			turnController.OnTurnChanged += OnTurnChanged;
		}

		public CityState GetCityState(string cityName) {
			var state = _mapState.Cities.Find(x => x.CityName == cityName);
			Assert.IsNotNull(state);
			return state;
		}

		public Dictionary<ResourceType, int> GetCityIncome(string cityName) {
			return GetCityIncome(GetCityState(cityName));
		}

		public Dictionary<UnitType, int> GetNotBoughtCityUnits(string cityName) {
			return GetCityState(cityName).ReadyToBuyUnits;
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
	}
}