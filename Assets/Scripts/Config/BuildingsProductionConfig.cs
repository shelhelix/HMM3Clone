using System;
using System.Collections.Generic;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using UnityEngine;

namespace Hmm3Clone.Config {
	[CreateAssetMenu]
	public class BuildingsProductionConfig : ScriptableObject {
		public List<BuildingProductionInfo> ProductionInfos;

		public List<BuildingUnitProductionInfo> UnitProductionInfos;

		public BuildingUnitProductionInfo GetUnitProductionInfo(BuildingType buildingType) {
			return UnitProductionInfos.Find(x => x.BuildingType == buildingType);
		}

		public BuildingProductionInfo GetProductionInfo(BuildingType buildingName) {
			return ProductionInfos.Find(x => x.BuildingType == buildingName);
		}
	}

	[Serializable]
	public class BuildingUnitProductionInfo {
		public BuildingType BuildingType;
		public UnitType UnitType;
		public int Amount;
	}
}