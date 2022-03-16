using System;
using System.Collections.Generic;
using Hmm3Clone.Controller;

namespace Hmm3Clone.Config.Map {
	[Serializable]
	public class MapNeutralArmyInfo : BaseMapObject, IDestructibleMapObject {
		public List<MapUnitInfo> UnitsStacks;
	}
}