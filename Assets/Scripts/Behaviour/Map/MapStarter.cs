using System.Collections.Generic;
using GameComponentAttributes;
using Hmm3Clone.Behaviour.Common;
using Hmm3Clone.Manager;
using Hmm3Clone.Scopes;
using Hmm3Clone.State;
using UnityEngine;
using VContainer;

namespace Hmm3Clone.Behaviour.Map {
	[DefaultExecutionOrder(-1000)]
	public class MapStarter : GameComponent {
		[Inject] 
		GameState _state;

		[Inject] 
		RuntimeMapInfo _mapInfo;

		[Inject] 
		MapManager _mapManager;

		[Inject] MapScope Scope;
		
		public void Start() {
			_mapInfo.CompressMap();
			if (!_state.FirstInitializationCompleted) {
				_mapInfo.GameplayMapInfo.MapCities.ForEach(x => _state.MapState.CityStates.Add(new CityState(x.CityName)));
				_state.FirstInitializationCompleted = true;
			}
			
			_mapManager.Init();
			
			var copy = new List<BaseInjectableComponent>(BaseInjectableComponent.Components);
			copy.ForEach(x => Scope.Container.Inject(x));
		}
	}
}