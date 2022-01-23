using System.Collections.Generic;
using Hmm3Clone.Config;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using UnityEngine;

namespace Hmm3Clone {
	
	[CreateAssetMenu]
	public class BuildingInfo : ScriptableObject {
		[Header("INGAME INFO")]
		public BuildingType               Name;
		public List<Resource>             BuildingCost;
		public List<BuildingInfo>         Dependencies;
		public List<Resource>             ResourcesProduction;
		public List<BuildingUnitProductionInfo> UnitProductionInfo;
		[Header("SPRITES")]
		public Sprite                 BuildingSprite;
	}
}