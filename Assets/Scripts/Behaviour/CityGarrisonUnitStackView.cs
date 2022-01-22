using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.State;
using TMPro;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
	public class CityGarrisonUnitStackView : GameComponent {
		[NotNull] public Image    Image;
		[NotNull] public TMP_Text AmountText;

		public void InitView(UnitType unitType, int amount) {
			AmountText.text = amount.ToString();
			SetActive(true);
		}

		public void SetActive(bool isActive) {
			Image.enabled      = isActive;
			AmountText.enabled = isActive;
		}
	}
}