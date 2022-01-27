using System;
using System.Collections.Generic;

namespace Hmm3Clone.State {
	[Serializable]
	public class HeroControllerState {
		public List<HeroState> Heroes = new List<HeroState>();

		public HeroState GetHeroState(string heroName) {
			return Heroes.Find(x => x.HeroName == heroName);
		}
	}
}