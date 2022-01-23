using System;
using System.Collections.Generic;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using UnityEngine;

namespace Hmm3Clone.Config {

	[Serializable]
	public class BuildingUnitProductionInfo {
		public UnitType UnitType;
		public int Amount;
	}
}