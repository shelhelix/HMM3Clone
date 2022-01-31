using System;
using UnityEngine;

namespace Hmm3Clone.State {
	[Serializable]
	public class HeroState {
		public string      HeroName;
		public UnitStack[] Stacks = new UnitStack[CityState.MaxUnitStacksCount];
		public Vector3Int  MapPosition;
		
		public HeroState() {}

		public HeroState(string heroName) {
			HeroName = heroName;
		}
	}
}