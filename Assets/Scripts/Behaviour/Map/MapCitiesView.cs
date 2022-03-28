using System.Collections.Generic;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.Manager;
using Hmm3Clone.Utils;
using VContainer;

namespace Hmm3Clone.Behaviour.Map {
	public class MapCitiesView : GameComponent {
		[NotNullOrEmpty] public List<MapCityView> Cities;

		[Inject] CityController _cityController;
		
		[Inject]
		void Init(MapManager manager) {
			var allCities = _cityController.GetAllCities();
			foreach (var (view, state) in Cities.MyZip(allCities)) {
				view.gameObject.SetActive(state != null);
				if (view.gameObject.activeSelf) {
					view.Init(manager, state.CityName);
				}
				else {
					view.Deinit();
				}
			}
		}
	}
}