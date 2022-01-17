using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.State;
using UnityEngine;

namespace Hmm3Clone.Behaviour {
	public class CitySceneStarter : GameComponent {
		[NotNull] public GameObject BuildingsScreen;

		protected override void Awake() {
			base.Awake();
			var cityState = new CityState();
			cityState.ErectBuilding("TownHall");
			ActiveData.Instance.SetData(cityState);
		}
		
		public void Start() {
			BuildingsScreen.gameObject.SetActive(false);
		}
	}
}