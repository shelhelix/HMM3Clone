using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using UnityEngine;

namespace Hmm3Clone.Behaviour {
	public class CitySceneStarter : GameComponent {
		[NotNull] public GameObject ScreenRoot;

		protected override void Awake() {
			base.Awake();
			var cityState = GameController.Instance.GetController<CityController>().GetCityState(CityController.TestCityName);
			ActiveData.Instance.SetData(cityState);
		}
		
		public void Start() {
			foreach (Transform child in ScreenRoot.transform) {
				child.gameObject.SetActive(false);
			}
		}
	}
}