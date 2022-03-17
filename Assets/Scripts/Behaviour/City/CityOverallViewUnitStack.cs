using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.SpriteSetups;
using Hmm3Clone.State;
using TMPro;
using UnityEngine.UI;
using VContainer;

namespace Hmm3Clone.Behaviour {
	public class CityOverallViewUnitStack : GameComponent {
		[NotNull] public Image    UnitImage;
		[NotNull] public TMP_Text AmountText;
		[NotNull] public Button   Button;
		
		[NotNull(false)] public HiringWindow HiringWindow;

		[Inject] SpriteSetupController _spriteSetupController;
		[Inject] UnitsController       _unitsController;
		[Inject] CityController        _cityController;
		[Inject] CityState             _cityState;
		
		public void Init(UnitType unitType, int unitCount) {
			AmountText.text = unitCount.ToString();
			Button.onClick.RemoveAllListeners();
			Button.onClick.AddListener(() => HiringWindow.Init(unitType));
			var advancedForm = _unitsController.GetAdvancedUnitType(unitType);
			UnitImage.sprite = _spriteSetupController.GetSpriteSetup<UnitsSpriteSetup>().GetCityOverviewSprite(_cityController.CanHireUnit(_cityState.CityName, advancedForm) ? advancedForm : unitType);
			SetActiveInternalObjects(true);
		}

		public void SetActiveInternalObjects(bool isActive) {
			UnitImage.enabled = isActive;
			AmountText.enabled = isActive;
		}
	}
}