using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

namespace Hmm3Clone.Behaviour {
	public class GarrisonUnitDragger : BaseDragger<GarrisonUnitDragger, CityGarrisonUnitStackView> {
		[NotNull] public Toggle SplitToggle;

		[Inject] CityController _cityController;
		[Inject] CityState      _activeCity;
		
		public override void OnEndDrag(PointerEventData data) {
			var overlappedViews = DoRaycast(data);
			if (overlappedViews.Count > 0) {
				var otherUnitStackView = overlappedViews[0];
				if (SplitToggle.isOn) {
					TrySplitStacks(StartItem, otherUnitStackView);
				}
				else {
					_cityController.TransformStacks(_activeCity.CityName, StartItem.Index, otherUnitStackView.Index);
				}
			}
			StartItem = null;
		}

		void TrySplitStacks(CityGarrisonUnitStackView source, CityGarrisonUnitStackView dest) {
			SplitToggle.isOn = !_cityController.TrySplitStacks(_activeCity.CityName, source.Index, dest.Index);
		}
	}
}