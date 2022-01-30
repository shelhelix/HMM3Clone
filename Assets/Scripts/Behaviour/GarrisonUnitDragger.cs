using Hmm3Clone.Controller;
using Hmm3Clone.State;
using UnityEngine.EventSystems;

namespace Hmm3Clone.Behaviour {
	public class GarrisonUnitDragger : BaseDragger<GarrisonUnitDragger, CityGarrisonUnitStackView> {
		CityController _cityController;

		CityState _activeCity;
		
		protected override void Start() {
			base.Start();
			_cityController = GameController.Instance.GetController<CityController>();
			_activeCity     = ActiveData.Instance.GetData<CityState>();
		}

		public override void OnEndDrag(PointerEventData data) {
			var overlappedViews = DoRaycast(data);
			if (overlappedViews.Count > 0) {
				var otherUnitStackView = overlappedViews[0];
				_cityController.TransformStacks(_activeCity.CityName, StartItem.Index, otherUnitStackView.Index);
			}
			StartItem = null;
		}
	}
}