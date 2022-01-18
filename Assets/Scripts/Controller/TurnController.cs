using System;
using Hmm3Clone.State;

namespace Hmm3Clone.Controller {
	public class TurnController : IController {
		TurnState _state;

		public int Turn => _state.Turn;

		public event Action OnTurnChanged;
		
		public TurnController(TurnState state) {
			_state = state;
		}


		public void EndTurn() {
			_state.Turn++;
			OnTurnChanged?.Invoke();
		}
	}
}