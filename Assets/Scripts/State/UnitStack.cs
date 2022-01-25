namespace Hmm3Clone.State {
	public class UnitStack {
		public UnitType Type;
		public int Amount;

		public UnitStack() { }

		public UnitStack(UnitType unitType, int amount) {
			Type   = unitType;
			Amount = amount;
		}
	}
}