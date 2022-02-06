using System;
using System.Collections.Generic;
using Hmm3Clone.Controller;

namespace Hmm3Clone.Config.Map {
	[Serializable]
	public class MapCityConstructionInfo : BaseMapObject {
		public string             CityName;
		public List<BuildingType> PrebuildBuildings;
	}
}