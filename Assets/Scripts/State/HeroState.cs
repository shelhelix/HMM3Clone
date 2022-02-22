using System;
using UnityEngine;

namespace Hmm3Clone.State {
	[Serializable]
	public class HeroState {
		public static readonly Vector3Int InvalidPoint = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
		
		public       string      HeroName;
		public       UnitStack[] Stacks = new UnitStack[CityState.MaxUnitStacksCount];
		
		// Data for map only
		public Vector3Int MapPosition;
		public int        LeftMovementPoints;
		public Vector3Int PathEndPoint = InvalidPoint;

		public HeroState() {}

		public HeroState(string heroName) {
			HeroName = heroName;
		}
	}
}