using GameComponentAttributes;
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

		[Inject] CityController _cityController;
		
		public void Start() {
			_mapInfo.CompressMap();
			if (!_state.FirstInitializationCompleted) {
				_mapInfo.GameplayMapInfo.MapCities.ForEach(x => {
					_cityController.AddCity(x.CityName);
					foreach (var building in x.PrebuildBuildings) {
						_cityController.ErectBuilding(x.CityName, building, true);
					}
				});
				_state.FirstInitializationCompleted = true;
			}
			
			_mapManager.Init();
		}
	}
}