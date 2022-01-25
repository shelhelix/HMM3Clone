using System.Collections.Generic;
using Hmm3Clone.State;
using UnityEngine;

namespace Hmm3Clone.Config {
	[CreateAssetMenu]
	public class UnitInfo : ScriptableObject {
		public UnitType       UnitType;
		public List<Resource> HirePrice;
		
		public UnitInfo       UnitAdvancedForm;
	}
}