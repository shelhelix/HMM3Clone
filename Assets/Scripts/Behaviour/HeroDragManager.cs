using Hmm3Clone.Controller;
using Hmm3Clone.State;
using UnityEngine.EventSystems;

namespace Hmm3Clone.Behaviour {
	public class HeroDragManager : BaseDragger<HeroDragManager, HeroAvatarView> {
		CityController _cityController;

		CityState _activeCity;
		
		protected override void Start() {
			base.Start();
			_cityController = GameController.Instance.GetController<CityController>();
			_activeCity     = ActiveData.Instance.GetData<CityState>();
		}

		public override void OnEndDrag(PointerEventData data) {
			var overlappedObjects = DoRaycast(data);
			if (overlappedObjects.Count > 0) {
				var obj = overlappedObjects[0];
				if (obj == StartItem) {
					return;
				}
				_cityController.TrySwapHeroesInCity(_activeCity.CityName, StartItem.Source, obj.Source);
			}
			StartItem = null;
		}
	}
}