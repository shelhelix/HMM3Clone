using System.Collections.Generic;
using Hmm3Clone.Controller;
using UnityEngine;

namespace Hmm3Clone.Config {
	[CreateAssetMenu]
	public class BuildingsProductionConfig : ScriptableObject {
		public List<BuildingProductionInfo> ProductionInfos;

		public BuildingProductionInfo GetProductionInfo(BuildingType buildingName) {
			return ProductionInfos.Find(x => x.BuildingType == buildingName);
		}
	}
}