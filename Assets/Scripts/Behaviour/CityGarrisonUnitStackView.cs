using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.SpriteSetups;
using Hmm3Clone.State;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace Hmm3Clone.Behaviour {
	public class CityGarrisonUnitStackView : BaseDraggableUiItem<CityGarrisonUnitStackView, GarrisonUnitDragger> {
		[NotNull] public Image    Image;
		[NotNull] public TMP_Text AmountText;

		public CityUnitStackIndex Index;

		[Inject] SpriteSetupController _spriteSetupController;
		
		UnitsSpriteSetup _spriteSetup;

		bool _inited;
		
		void Start() {
			if (_inited) {
				return;
			}
			_spriteSetup = _spriteSetupController.GetSpriteSetup<UnitsSpriteSetup>();
			_inited      = true;
		}

		public void InitCommonView(CityUnitStackIndex index) {
			Start();
			Index = index;
		}
		
		public void InitView(UnitStack unitStack) {
			AmountText.text = unitStack.Amount.ToString();
			Image.sprite    = _spriteSetup.GetGarrisonSprite(unitStack.Type);
			SetActive(true);
		}

		public void SetActive(bool isActive) {
			MovableRoot.gameObject.SetActive(isActive);
		}
	}
}