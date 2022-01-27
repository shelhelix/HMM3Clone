using System;
using Hmm3Clone.Behaviour;

namespace Hmm3Clone.Controller {
	public struct CityUnitStackIndex : IEquatable<CityUnitStackIndex> {
		public ArmySource ArmySource;
		public int        StackIndex;

		public CityUnitStackIndex(ArmySource armySource, int stackIndex) {
			ArmySource = armySource;
			StackIndex = stackIndex;
		}

		public bool Equals(CityUnitStackIndex other) {
			return ArmySource == other.ArmySource && StackIndex == other.StackIndex;
		}

		public override bool Equals(object obj) {
			return obj is CityUnitStackIndex other && Equals(other);
		}

		public override int GetHashCode() {
			unchecked {
				return ((int) ArmySource * 397) ^ StackIndex;
			}
		}
	}
}