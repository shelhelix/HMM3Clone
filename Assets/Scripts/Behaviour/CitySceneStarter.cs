using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using UnityEngine;

namespace Hmm3Clone.Behaviour {
	public class CitySceneStarter : GameComponent {
		[NotNull] public GameObject BuildingsScreen;

		protected override void Awake() {
			base.Awake();
			var cityState = GameController.Instance.GetController<CityController>().GetCityState(CityController.TestCityName);
			ActiveData.Instance.SetData(cityState);
		}
		
		public void Start() {
			BuildingsScreen.gameObject.SetActive(false);
		}
	}
}