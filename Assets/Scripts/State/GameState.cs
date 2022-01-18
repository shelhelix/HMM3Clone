using System;

namespace Hmm3Clone.State {
	[Serializable]
	public class GameState {
		public ResourcesState ResourcesState = new ResourcesState();
		public TurnState TurnState = new TurnState();
		public MapState MapState = new MapState();
	}
}