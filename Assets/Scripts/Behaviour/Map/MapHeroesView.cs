using System.Collections.Generic;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Behaviour.Common;
using Hmm3Clone.Controller;
using Hmm3Clone.Manager;
using Hmm3Clone.Utils;
using VContainer;

namespace Hmm3Clone.Behaviour.Map {
	public class MapHeroesView : BaseInjectableComponent {
		[NotNullOrEmpty] public List<MapHeroView> HeroViews;


		[Inject]
		void Init(MapManager manager) {
			var heroController = GameController.Instance.GetController<HeroController>();
			var allHeroesOnMap = heroController.GetAllHeroes();

			foreach (var (view, state) in HeroViews.MyZip(allHeroesOnMap)) {
				view.gameObject.SetActive(state != null);
				if (state != null) {
					view.Init(manager, heroController, state.HeroName);
				}
				else {
					view.Deinit();
				}
			}
		}
	}
}