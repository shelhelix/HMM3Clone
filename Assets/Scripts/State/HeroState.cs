using System;
using UnityEngine;

namespace Hmm3Clone.State {
	[Serializable]
	public class HeroState {
		public string      HeroName;
		public UnitStack[] Stacks = new UnitStack[CityState.MaxUnitStacksCount];
		
		// Data for map only
		public Vector3Int MapPosition;
		public int        LeftMovementPoints;

		public HeroState() {}

		public HeroState(string heroName) {
			HeroName = heroName;
		}
	}
}