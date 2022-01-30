using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.SpriteSetups;
using Hmm3Clone.State;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
	public class CityGarrisonUnitStackView : BaseDraggableUiItem<CityGarrisonUnitStackView, GarrisonUnitDragger>, IPointerClickHandler {
		[NotNull] public Image    Image;
		[NotNull] public TMP_Text AmountText;

		public CityUnitStackIndex Index;
		
		UnitsSpriteSetup _spriteSetup;
		
		public bool IsActive => MovableRoot.gameObject.activeSelf;
		
		bool _inited;
		
		void Start() {
			if (_inited) {
				return;
			}
			_spriteSetup = GameController.Instance.GetController<SpriteSetupController>()
										 .GetSpriteSetup<UnitsSpriteSetup>();
			_inited = true;
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

		public void OnPointerClick(PointerEventData eventData) {
			GarrisonUnitSplitter.Instance.OnUnitStackSelected(this);
		}
	}
}