using Hmm3Clone.Gameplay;

namespace Hmm3Clone.Scopes.Mediators {
	public enum SideType {
		Neutral,
		Hero,
		City
	}
	
	public readonly struct BattleSideInfo {
		public readonly SideType Type;
		// Type == City => Name is a city name
		// Type == Hero => Name is a hero name
		// Type == Neutral => Name is a neutral army position
		public readonly string   Name;
		public readonly Army     Army;

		public readonly bool IsValid;

		public static BattleSideInfo InvalidSide => new BattleSideInfo();
		
		public BattleSideInfo(string name, Army army, SideType type) {
			Name    = name;
			Army    = army;
			Type    = type;
			IsValid = true;
		}
	}
}