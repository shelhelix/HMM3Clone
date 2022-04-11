using System;
using Hmm3Clone.State;

namespace Hmm3Clone.Config.Map {
	[Serializable]
	public class MapNeutralArmyInfo : BaseMapObject, IDestructibleMapObject {
		public UnitStack[] UnitsStacks;
	}
}