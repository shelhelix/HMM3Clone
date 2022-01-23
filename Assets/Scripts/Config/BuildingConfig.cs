using System.Collections.Generic;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hmm3Clone.Config {
	[CreateAssetMenu(fileName = nameof(BuildingConfig))]
	public class BuildingConfig : ScriptableObject {
		public List<BuildingInfo> Buildings;

		#if UNITY_EDITOR
		[Button] [UsedImplicitly]
		public void Refresh() {
			var buildings = Resources.LoadAll<BuildingInfo>("Buildings");
			Buildings = new List<BuildingInfo>(buildings);
		}
		#endif

		public List<BuildingUnitProductionInfo> GetUnitProductionInfo(BuildingType buildingType) {
			return GetBuildingInfo(buildingType).UnitProductionInfo;
		}

		public List<Resource> GetResourcesProductionInfo(BuildingType buildingType) {
			return GetBuildingInfo(buildingType).ResourcesProduction;
		}

		BuildingInfo GetBuildingInfo(BuildingType buildingType) {
			var res = Buildings.Find(x => x.Name == buildingType);
			Assert.IsTrue(res);
			return res;
		}
	}
}