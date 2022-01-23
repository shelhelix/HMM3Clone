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
		
		void Start() {
			_spriteSetup = GameController.Instance.GetController<SpriteSetupController>()
										 .GetSpriteSetup<UnitsSpriteSetup>();
		}

		public void Init(UnitType unitType, int unitCount) {
			AmountText.text = unitCount.ToString();
			Button.onClick.RemoveAllListeners();
			Button.onClick.AddListener(() => HiringWindow.Init(unitType));
			UnitImage.sprite = _spriteSetup.GetCityOverviewSprite(unitType);
			SetActiveInternalObjects(true);
		}

		public void SetActiveInternalObjects(bool isActive) {
			UnitImage.enabled = isActive;
			AmountText.enabled = isActive;
		}
	}
}