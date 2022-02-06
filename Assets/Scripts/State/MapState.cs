using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hmm3Clone.State {
	[Serializable]
	public class MapState {
		public string             MapName;
		public List<CityState>    CityStates            = new List<CityState>();
		public List<Vector2Int>   RemovedObjectsFromMap = new List<Vector2Int>();
	}
}