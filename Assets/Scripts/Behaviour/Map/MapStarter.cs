using GameComponentAttributes;
using Hmm3Clone.Config.Map;
using Hmm3Clone.Controller;
using Hmm3Clone.Manager;
using Hmm3Clone.State;
using UnityEngine;
using VContainer;

namespace Hmm3Clone.Behaviour.Map {
	[DefaultExecutionOrder(-1000)]
	public class MapStarter : GameComponent {
		[Inject] GameState _state;

		[Inject] RuntimeMapInfo _mapInfo;

		[Inject] MapManager _mapManager;

		[Inject] CityController          _cityController;
		[Inject] DeadMapObjectsController _deadMapObjectsController;
		
		public void Start() {
			_mapInfo.CompressMap();
			FirstMapInit();
			_mapManager.Init();
		}

		void FirstMapInit() {
			if (_state.FirstInitializationCompleted) {
				return;
			}
			_mapInfo.GameplayMapInfo.MapCities.ForEach(AddCity);
			
			_state.FirstInitializationCompleted = true;
		}

		void AddCity(MapCityConstructionInfo info) {
			_cityController.AddCity(info.CityName);
			foreach (var building in info.PrebuildBuildings) {
				_cityController.ErectBuilding(info.CityName, building, true);
			}
		}
	}
}