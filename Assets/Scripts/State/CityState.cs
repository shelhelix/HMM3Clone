using System.Collections.Generic;

namespace Hmm3Clone.State {
	public class CityState {
		public List<string> ErectedBuildings = new List<string>();

		public bool IsErected(string buildingName) {
			return ErectedBuildings.Contains(buildingName);
		}

		public void ErectBuilding(string buildingName) {
			ErectedBuildings.Add(buildingName);
		}
	}
}
