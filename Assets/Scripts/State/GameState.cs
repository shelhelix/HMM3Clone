using System;

namespace Hmm3Clone.State {
	[Serializable]
	public class GameState {
		public bool                FirstInitializationCompleted;
		public ResourcesState      ResourcesState = new ResourcesState();
		public TurnState           TurnState      = new TurnState();
		public MapState            MapState       = new MapState();
		public HeroControllerState HeroState      = new HeroControllerState();
	}
}