using Hmm3Clone.Controller;
using Hmm3Clone.State;
using UnityEngine.EventSystems;
using VContainer;

namespace Hmm3Clone.Behaviour {
	public class HeroDragManager : BaseDragger<HeroDragManager, HeroAvatarView> {
		[Inject] CityController _cityController;
		[Inject] CityState      _cityState;

		public override void OnEndDrag(PointerEventData data) {
			var overlappedObjects = DoRaycast(data);
			if (overlappedObjects.Count > 0) {
				var obj = overlappedObjects[0];
				if (obj == StartItem) {
					return;
				}
				_cityController.TrySwapHeroesInCity(_cityState.CityName, StartItem.Source, obj.Source);
			}
			StartItem = null;
		}
	}
}