using System.Collections.Generic;
using Hmm3Clone.State;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hmm3Clone.SpriteSetups {
	[CreateAssetMenu]
	public class UnitsSpriteSetup : ScriptableObject {
		public List<UnitSpriteContainer> UnitSprites;

		public Sprite GetGarrisonSprite(UnitType unitType) {
			return GetSpriteContainer(unitType)?.GarrisonSprite;
		}

		public Sprite GetCityOverviewSprite(UnitType unitType) {
			return GetSpriteContainer(unitType)?.CityOverviewSprite;
		}

		public Sprite GetHireSprite(UnitType unitType) {
			return GetSpriteContainer(unitType)?.HireSprite;
		}

		UnitSpriteContainer GetSpriteContainer(UnitType unitType) {
			var res = UnitSprites.Find(x => x.UnitType == unitType);
			Assert.IsNotNull(res);
			return res;
		}
	}
}