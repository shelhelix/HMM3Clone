using Hmm3Clone.Config;
using Hmm3Clone.State;
using Hmm3Clone.Utils;
using UnityEngine.Assertions;

namespace Hmm3Clone.Controller {
	public class UnitsController : IController {
		UnitInfoConfig _config;

		public UnitsController() {
			_config = ConfigLoader.LoadConfig<UnitInfoConfig>();
		}

		public UnitInfo GetUnitInfo(UnitType unitType) {
			return _config.GetUnitInfo(unitType);
		}

		public UnitInfo GetAdvancedUnitInfo(UnitType unitType) {
			var unitInfo = _config.GetUnitInfo(unitType);
			Assert.IsTrue(unitInfo.UnitAdvancedForm);
			return unitInfo.UnitAdvancedForm;
		}

		public bool IsAdvancedUnit(UnitType unitType) {
			return _config.Units.Exists(x => x.UnitAdvancedForm && x.UnitAdvancedForm.UnitType == unitType);
		}

		public UnitType GetAdvancedUnitType(UnitType unitType) {
			var unitInfo = _config.GetUnitInfo(unitType);
			return unitInfo.UnitAdvancedForm ? unitInfo.UnitAdvancedForm.UnitType : UnitType.None;
		}

		public UnitType GetBaseUnitType(UnitType unitType) {
			if (!IsAdvancedUnit(unitType)) {
				return unitType;
			}
			var baseUnitInfo = _config.Units.Find(x => x.UnitAdvancedForm.UnitType == unitType);
			return baseUnitInfo.UnitType;
		}
	}
}