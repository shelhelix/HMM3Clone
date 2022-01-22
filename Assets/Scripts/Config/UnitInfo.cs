using System;
using System.Collections.Generic;
using Hmm3Clone.State;

namespace Hmm3Clone.Config {
	[Serializable]
	public class UnitInfo {
		public UnitType       UnitType;
		public List<Resource> HirePrice;
	}
}