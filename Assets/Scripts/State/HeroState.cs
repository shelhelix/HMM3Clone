using System;

namespace Hmm3Clone.State {
	[Serializable]
	public class HeroState {
		public string      HeroName;
		public UnitStack[] Stacks = new UnitStack[CityState.MaxUnitStacksCount];
		
		public HeroState() {}

		public HeroState(string heroName) {
			HeroName = heroName;
		}
	}
}