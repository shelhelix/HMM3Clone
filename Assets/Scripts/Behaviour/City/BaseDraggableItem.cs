using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hmm3Clone.Behaviour {
	public abstract class BaseDraggableUiItem<TActualType, TDraggerType> : GameComponent, IBeginDragHandler, IEndDragHandler, IDragHandler 
		where TDraggerType : BaseDragger<TDraggerType, TActualType> where TActualType : GameComponent {
		[NotNull] public Transform MovableRoot;
		[NotNull] public Canvas    Canvas;

		public bool IsActive => MovableRoot.gameObject.activeSelf;
		
		Vector3 _startPosition;
		int     _startSortingOrder;
		
		public void OnBeginDrag(PointerEventData eventData) {
			if (!IsActive) {
				return;
			}
			BaseDragger<TDraggerType, TActualType>.Instance.OnBeginDrag(this as TActualType);
			_startPosition      = MovableRoot.position;
			_startSortingOrder  = Canvas.sortingOrder;
			Canvas.sortingOrder = 30000;
		}

		public void OnEndDrag(PointerEventData eventData) {
			if (!IsActive) {
				return;
			}
			MovableRoot.position = _startPosition;
			Canvas.sortingOrder  = _startSortingOrder;
			BaseDragger<TDraggerType, TActualType>.Instance.OnEndDrag(eventData);
		}

		public void OnDrag(PointerEventData eventData) {
			if (!IsActive) {
				return;
			}
			var pos = Camera.main.ScreenToWorldPoint(eventData.position);
			pos.z                = _startPosition.z;
			MovableRoot.position = pos;
		}
	}
}