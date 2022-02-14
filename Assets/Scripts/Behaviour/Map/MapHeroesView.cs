using System.Collections.Generic;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.Utils;

namespace Hmm3Clone.Behaviour.Map {
	public class MapHeroesView : GameComponent {
		[NotNullOrEmpty] public List<MapHeroView> HeroViews;
		
		void Start() {
			var heroController = GameController.Instance.GetController<HeroController>();
			var allHeroesOnMap = heroController.GetAllHeroes();

			foreach (var (view, state) in HeroViews.MyZip(allHeroesOnMap)) {
				view.gameObject.SetActive(state != null);
				if (state != null) {
					view.Init(heroController, state.HeroName);
				}
			}
		}
	}
}