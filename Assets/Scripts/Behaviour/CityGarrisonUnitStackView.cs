
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.Gameplay;
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

		public CityUnitStackIndex Index;
		
		UnitsSpriteSetup _spriteSetup;

		Vector3     _startPosition;
		int         _startSortingOrder;
		
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

		public void OnBeginDrag(PointerEventData eventData) {
			GarrisonUnitDragger.Instance.OnGarrisonUnitBeginDrag(this);
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