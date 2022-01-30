using System;

namespace Hmm3Clone.State {
	[Serializable]
	public class UnitStack {
		public UnitType Type;
		public int Amount;

		public UnitStack() { }

		public UnitStack(UnitType unitType, int amount) {
			Type   = unitType;
			Amount = amount;
		}

		public UnitStack Clone() {
			return (UnitStack) MemberwiseClone();
		}
	}
}