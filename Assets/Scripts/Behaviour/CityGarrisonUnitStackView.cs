using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.SpriteSetups;
using Hmm3Clone.State;
using TMPro;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
	public class CityGarrisonUnitStackView : GameComponent {
		[NotNull] public Image    Image;
		[NotNull] public TMP_Text AmountText;

		UnitsSpriteSetup _spriteSetup;
		
		void Start() {
			_spriteSetup = GameController.Instance.GetController<SpriteSetupController>()
										 .GetSpriteSetup<UnitsSpriteSetup>();
		}
		
		public void InitView(UnitType unitType, int amount) {
			AmountText.text = amount.ToString();
			Image.sprite    = _spriteSetup.GetGarrisonSprite(unitType);
			SetActive(true);
		}

		public void SetActive(bool isActive) {
			Image.enabled      = isActive;
			AmountText.enabled = isActive;
		}
	}
}