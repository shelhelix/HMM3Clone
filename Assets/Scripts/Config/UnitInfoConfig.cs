using System.Collections.Generic;
using Hmm3Clone.State;
using UnityEngine;

namespace Hmm3Clone.Config {
	[CreateAssetMenu]
	public class UnitInfoConfig : ScriptableObject {
		public List<UnitInfo> Units;

		public UnitInfo GetUnitInfo(UnitType unitType) {
			return Units.Find(x => x.UnitType == unitType);
		}
	}
}