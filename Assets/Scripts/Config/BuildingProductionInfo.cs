using System;
using System.Collections.Generic;
using Hmm3Clone.Controller;
using Hmm3Clone.State;

namespace Hmm3Clone.Config {
	[Serializable]
	public class BuildingProductionInfo {
		public BuildingType   BuildingType;
		public List<Resource> ResourcesProduction;
	}
}