using System.Collections.Generic;
using System.Linq;
using GameComponentAttributes;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace Hmm3Clone.Behaviour {
	public class GarrisonUnitDragger : GameComponent {
		public static GarrisonUnitDragger Instance;

		CityController _cityController;

		CityState _activeCity;
		
		void Start() {
			_cityController = GameController.Instance.GetController<CityController>();
			_activeCity     = ActiveData.Instance.GetData<CityState>();
			Assert.IsFalse(Instance);
			Instance = this;
		}

		void OnDestroy() {
			if (Instance == this) {
				Instance = null;
			}
		}

		int _draggingStackIndex;

		public bool CanDrag;
		
		public void OnGarrisonUnitBeginDrag(int stackIndex) {
			_draggingStackIndex = stackIndex;
		}

		public void OnGarrisonUnitEndDrag(PointerEventData data) {
			var res      = new List<RaycastResult>();
			EventSystem.current.RaycastAll(data, res);
			var garrisonUnitsViews = res.Select(x => x.gameObject.GetComponent<CityGarrisonUnitStackView>()).Where(x => x).ToList();
			if (garrisonUnitsViews.Count > 0) {
				var otherUnitStackView = garrisonUnitsViews[0];
				_cityController.TransformStacks(_activeCity.CityName, _draggingStackIndex, otherUnitStackView.StackIndex);
			}
			_draggingStackIndex = -1;
		}
	}
}