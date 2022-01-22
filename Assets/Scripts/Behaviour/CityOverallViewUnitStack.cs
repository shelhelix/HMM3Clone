using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.State;
using TMPro;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
	public class CityOverallViewUnitStack : GameComponent {
		[NotNull] public Image    UnitImage;
		[NotNull] public TMP_Text AmountText;
		[NotNull] public Button   Button;
		
		[NotNull(false)] public HiringWindow HiringWindow;
		
		public void Init(UnitType unitType, int unitCount) {
			AmountText.text = unitCount.ToString();
			Button.onClick.RemoveAllListeners();
			Button.onClick.AddListener(() => HiringWindow.Init(unitType));
			SetActiveInternalObjects(true);
		}

		public void SetActiveInternalObjects(bool isActive) {
			UnitImage.enabled = isActive;
			AmountText.enabled = isActive;
		}
	}
}