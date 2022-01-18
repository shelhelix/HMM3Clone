using System;
using System.Collections.Generic;

namespace Hmm3Clone.State {
	[Serializable]
	public class MapState {
		public List<CityState> Cities = new List<CityState>();
	}
}