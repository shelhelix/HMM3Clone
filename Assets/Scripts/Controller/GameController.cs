using System.Collections.Generic;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using Hmm3Clone.Utils;

namespace Hmm3Clone {
	public class GameController : Singleton<GameController> {
		public GameState ActiveState;
		
		List<object> _controllers = new List<object>();

		public GameController() {
			ActiveState = GameState.LoadState();
			_controllers.Add(new ResourceController(ActiveState.ResourcesState));
		}

		public T GetController<T>() where T : class {
			return _controllers.Find(x => x is T) as T;
		}
	}
}