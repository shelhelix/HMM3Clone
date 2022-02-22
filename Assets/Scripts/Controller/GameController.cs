using System.Collections.Generic;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using Hmm3Clone.Utils;
using UnityEngine.Assertions;

namespace Hmm3Clone {
	public class GameController : Singleton<GameController> {
		public GameState ActiveState;
		
		List<IController> _controllers = new List<IController>();

		public void InitControllers(GameState state) {
			ActiveState = state;
			_controllers.Add(new ResourceController(ActiveState.ResourcesState));
			_controllers.Add(new TurnController(ActiveState.TurnState));
			_controllers.Add(new UnitsController());
			_controllers.Add(new HeroController(GetController<TurnController>(), ActiveState.HeroState));
			_controllers.Add(new CityController(GetController<ResourceController>(),  GetController<UnitsController>(), 
				GetController<HeroController>(), GetController<TurnController>(), ActiveState.MapState));
			_controllers.Add(new SpriteSetupController());
		}

		public T GetController<T>() where T : class, IController {
			var res = _controllers.Find(x => x is T) as T;
			Assert.IsNotNull(res);
			return res;
		}
	}
}