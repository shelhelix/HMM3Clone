using System;
using System.Collections.Generic;
using Hmm3Clone.Controller;
using UnityEngine;

namespace Hmm3Clone.State {
	[Serializable]
	public class CityState {
		public const int MaxUnitStacksCount = 7;
		
		public string                    CityName;
		public List<BuildingType>        ErectedBuildings = new List<BuildingType>();
		public Dictionary<UnitType, int> ReadyToBuyUnits  = new Dictionary<UnitType, int>();
		public UnitStack[]               Garrison         = new UnitStack[MaxUnitStacksCount];
		
		public string HeroInGarrison;
		public string GuestHero;

		public bool CanErectBuilding;

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

		public void EraseGarrison() {
			Garrison = new UnitStack[MaxUnitStacksCount];
		}
	}
}
