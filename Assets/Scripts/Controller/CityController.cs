using Hmm3Clone.Config;
using Hmm3Clone.State;
using Hmm3Clone.Utils;
using UnityEngine.Assertions;

namespace Hmm3Clone.Controller {
	public class CityController : IController {
		public const string TestCityName = "TestCity";
		
		const string BaseBuildingName = "TownHall";
		
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

		void OnTurnChanged() {
			ProduceResources();
		}
		
		void ProduceResources() {
			foreach (var cityState in _mapState.Cities) {
				foreach (var buildingName in cityState.ErectedBuildings) {
					var productionInfo = _productionConfig.GetProductionInfo(buildingName);
					productionInfo?.ResourcesProduction.ForEach(x => _resourceController.AddResource(x));
				}
			}
		}

		public CityState GetCityState(string cityName) {
			var state = _mapState.Cities.Find(x => x.CityName == cityName);
			Assert.IsNotNull(state);
			return state;
		}

		CityState CreateCityState(string cityName) {
			var state = new CityState(cityName);
			state.ErectBuilding(BuildingType.TownHall);
			return state;
		}
	}
}