
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.SpriteSetups;
using Hmm3Clone.State;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
	public class CityGarrisonUnitStackView : GameComponent, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler {
		[NotNull] public Image    Image;
		[NotNull] public TMP_Text AmountText;

		[NotNull] public Transform MovableRoot;
		[NotNull] public Canvas    Canvas;
		
		UnitsSpriteSetup _spriteSetup;

		public int StackIndex = -1;
		

		Vector3 _startPosition;
		int     _startSortingOrder;

		void Start() {
			_spriteSetup = GameController.Instance.GetController<SpriteSetupController>()
										 .GetSpriteSetup<UnitsSpriteSetup>();
		}

		public void InitStackIndex(int stackIndex) {
			StackIndex = stackIndex;
		}
		
		public void InitView(UnitStack unitStack) {
			AmountText.text = unitStack.Amount.ToString();
			Image.sprite    = _spriteSetup.GetGarrisonSprite(unitStack.Type);
			SetActive(true);
		}

		public void SetActive(bool isActive) {
			MovableRoot.gameObject.SetActive(isActive);
		}

		public void OnBeginDrag(PointerEventData eventData) {
			GarrisonUnitDragger.Instance.OnGarrisonUnitBeginDrag(StackIndex);
			_startPosition      = MovableRoot.position;
			_startSortingOrder  = Canvas.sortingOrder;
			Canvas.sortingOrder = 32760;
		}

		public void OnDrag(PointerEventData eventData) {
			var pos = Camera.main.ScreenToWorldPoint(eventData.position);
			pos.z                = _startPosition.z;
			MovableRoot.position = pos;
		}


		public void OnEndDrag(PointerEventData eventData) {
			GarrisonUnitDragger.Instance.OnGarrisonUnitEndDrag(eventData);
			MovableRoot.position = _startPosition;
			Canvas.sortingOrder  = _startSortingOrder;
		}

		public void OnPointerClick(PointerEventData eventData) {
			GarrisonUnitSplitter.Instance.OnUnitStackSelected(this);
		}
	}
}