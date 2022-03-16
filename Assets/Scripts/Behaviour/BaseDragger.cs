
using System.Collections.Generic;
using System.Linq;
using GameComponentAttributes;
using Hmm3Clone.Behaviour.Common;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Hmm3Clone.Behaviour {
	public abstract class BaseDragger<TDraggerType, TItemType> : BaseInjectableComponent where TDraggerType : GameComponent where TItemType : GameComponent {
		public static TDraggerType Instance;

		protected virtual void Start() {
			Assert.IsFalse(Instance);
			Instance = this as TDraggerType;
		}

		protected override void OnDestroy() {
			base.OnDestroy();
			if (Instance == this) {
				Instance = null;
			}
		}

		protected TItemType StartItem;

		public void OnBeginDrag(TItemType item) {
			StartItem = item;
		}

		public abstract void OnEndDrag(PointerEventData data);

		protected List<TItemType> DoRaycast(PointerEventData data) {
			var res = new List<RaycastResult>();
			EventSystem.current.RaycastAll(data, res);
			return res.Select(x => x.gameObject.GetComponent<TItemType>()).Where(x => x).ToList();
		} 
	}
}