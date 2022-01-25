using System;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.SpriteSetups;
using Hmm3Clone.State;
using TMPro;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
	public class CityOverallViewUnitStack : GameComponent {
		[NotNull] public Image    UnitImage;
		[NotNull] public TMP_Text AmountText;
		[NotNull] public Button   Button;
		
		[NotNull(false)] public HiringWindow HiringWindow;

		UnitsSpriteSetup _spriteSetup;
		UnitsController  _unitsController;
		CityController   _cityController;
		CityState        _state;
		
		void Start() {
			_spriteSetup = GameController.Instance.GetController<SpriteSetupController>()
										 .GetSpriteSetup<UnitsSpriteSetup>();
			_unitsController = GameController.Instance.GetController<UnitsController>();
			_cityController  = GameController.Instance.GetController<CityController>();
			_state           = ActiveData.Instance.GetData<CityState>();
		}

		public void Init(UnitType unitType, int unitCount) {
			AmountText.text = unitCount.ToString();
			Button.onClick.RemoveAllListeners();
			Button.onClick.AddListener(() => HiringWindow.Init(unitType));
			var advancedForm = _unitsController.GetAdvancedUnitType(unitType);
			UnitImage.sprite = _spriteSetup.GetCityOverviewSprite(_cityController.CanHireUnit(_state.CityName, advancedForm) ? advancedForm : unitType);
			SetActiveInternalObjects(true);
		}

		public void SetActiveInternalObjects(bool isActive) {
			UnitImage.enabled = isActive;
			AmountText.enabled = isActive;
		}
	}
}