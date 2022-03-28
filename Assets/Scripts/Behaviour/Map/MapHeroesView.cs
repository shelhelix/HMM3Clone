using System.Collections.Generic;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.Manager;
using Hmm3Clone.State;
using Hmm3Clone.Utils;
using VContainer;

namespace Hmm3Clone.Behaviour.Map {
	public class MapHeroesView : GameComponent {
		[NotNullOrEmpty] public List<MapHeroView> HeroViews;

		[Inject] CityController _cityController;
		
		[Inject]
		void Init(MapManager manager, HeroController heroController) {
			var allHeroesOnMap = heroController.GetAllHeroes();
			foreach (var (view, state) in HeroViews.MyZip(allHeroesOnMap)) {
				view.gameObject.SetActive(CanShow(state));
				if (view.gameObject.activeSelf) {
					view.Init(manager, heroController, state.HeroName);
				}
				else {
					view.Deinit();
				}
			}
		}

		bool CanShow(HeroState state) {
			return state != null && !_cityController.IsHeroInGarrison(state.HeroName);
		}
		
	}
}