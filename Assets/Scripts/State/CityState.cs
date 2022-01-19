using System;
using System.Collections.Generic;
using Hmm3Clone.Controller;

namespace Hmm3Clone.State {
	[Serializable]
	public class CityState {
		public string CityName;
		public List<BuildingType> ErectedBuildings = new List<BuildingType>();
		public Dictionary<UnitType, int> ReadyToBuyUnits = new Dictionary<UnitType, int>();

		public CityState() {
			CityName = "NoName";
		}
		
		public CityState(string cityName) {
			CityName = cityName;
		}

		public bool IsErected(BuildingType buildingName) {
			return ErectedBuildings.Contains(buildingName);
		}

		public void ErectBuilding(BuildingType buildingName) {
			ErectedBuildings.Add(buildingName);
		}
	}
}
