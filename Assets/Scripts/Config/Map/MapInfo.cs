using System.Collections.Generic;
using UnityEngine;

namespace Hmm3Clone.Config.Map {
	[CreateAssetMenu]
	public class MapInfo : ScriptableObject {
		[Header("static objects")]
		public List<MapCityConstructionInfo> MapCities;
		
		// [Header("consumable objects")]
		//
	}
}