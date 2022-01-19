using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using TMPro;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
	public class CityOverallViewUnitStack : GameComponent {
		[NotNull] public Image UnitImage;
		[NotNull] public TMP_Text AmountText;

		public void SetActiveInternalObjects(bool isActive) {
			UnitImage.enabled = isActive;
			AmountText.enabled = isActive;
		}
	}
}