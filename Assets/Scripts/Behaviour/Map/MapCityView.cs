using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.Gameplay;
using Hmm3Clone.Manager;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour.Map {
	public class MapCityView : GameComponent {
		[NotNull] public Button AvatarButton;

		string _cityName;
		
		MapManager _mapManager;
		
		public void Init(MapManager mapManager, string cityName) {
			_mapManager = mapManager;
			_cityName   = cityName;
			AvatarButton.onClick.AddListener(ShowCity);
		}

		public void Deinit() {
			AvatarButton.onClick.RemoveListener(ShowCity);
		}

		void ShowCity() {
			_mapManager.ShowCity(_cityName);	
		}
	}
}