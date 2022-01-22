using System;
using System.Collections.Generic;
using Hmm3Clone.Controller;

namespace Hmm3Clone.State {
	[Serializable]
	public class CityState {
		public const int MaxUnitStacksCount = 7;
		
		public string                    CityName;
		public List<BuildingType>        ErectedBuildings = new List<BuildingType>();
		public Dictionary<UnitType, int> ReadyToBuyUnits  = new Dictionary<UnitType, int>();
		public List<UnitStack>           Garrison         = new List<UnitStack>(MaxUnitStacksCount);

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

		public UnitStack GetUnitStack(UnitType unitType) {
			return Garrison.Find(x => x.Type == unitType);
		}
		
		public UnitStack GetOrCreateUnitStack(UnitType unitType) {
			var unitStack = GetUnitStack(unitType);
			if (unitStack != null) {
				return unitStack;
			}
			if (Garrison.Count == MaxUnitStacksCount) {
				return null;
			}
			unitStack = new UnitStack {Type = unitType};
			Garrison.Add(unitStack);
			return unitStack;
		}
	}
}
