using System.Collections.Generic;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using UnityEngine;

namespace Hmm3Clone {
	
	[CreateAssetMenu]
	public class BuildingInfo : ScriptableObject {
		public BuildingType Name;
		public List<Resource> BuildingCost;
		public List<BuildingInfo> Dependencies;
		public Sprite BuildingSprite;
	}
}