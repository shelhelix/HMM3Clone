namespace Hmm3Clone.State {
	public class BattleUnitStack {
		public UnitType Type;
		public int      Amount;
		
		//TODO: Add battle statuses
		
		public BattleUnitStack(UnitStack stack) {
			Type   = stack.Type;
			Amount = stack.Amount;
		}
	}
}