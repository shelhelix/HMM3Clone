using Hmm3Clone.State;

namespace Hmm3Clone.Gameplay {
	public class Hero {
		HeroState _state;

		public readonly Army Army; 

		public Hero(HeroState state) {
			_state = state;
			Army   = new Army(_state.Stacks);
		}
	}
}