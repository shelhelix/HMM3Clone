using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.State;
using UnityEngine;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
	public class CitySceneStarter : GameComponent {
		[NotNull] public Button OpenBuildingScreen;
		[NotNull] public GameObject BuildingsScreen;

		protected override void Awake() {
			base.Awake();
			var cityState = new CityState();
			cityState.ErectBuilding("TownHall");
			ActiveData.Instance.SetData(cityState);
		}
		
		public void Start() {
			BuildingsScreen.gameObject.SetActive(false);
			OpenBuildingScreen.onClick.AddListener(ShowBuildingScreen);
		}

		void ShowBuildingScreen() {
			BuildingsScreen.gameObject.SetActive(true);
		}
	}
}