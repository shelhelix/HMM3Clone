using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.DTO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
	public class CitySceneStarter : GameComponent {
		[NotNull] public GameObject ScreenRoot;

		[NotNull] public Button ReturnToMapButtom;
		protected override void Awake() {
			base.Awake();
			var initData = ActiveData.Instance.GetData<CityViewInitData>();
			ActiveData.Instance.RemoveData<CityViewInitData>();
			var cityState = GameController.Instance.GetController<CityController>().GetCityState(initData.CityName);
			ActiveData.Instance.SetData(cityState);
		}
		
		public void Start() {
			foreach (Transform child in ScreenRoot.transform) {
				child.gameObject.SetActive(false);
			}
			InitButtons();
		}

		void InitButtons() {
			ReturnToMapButtom.onClick.AddListener(ReturnToMap);
		}

		void ReturnToMap() {
			SceneManager.LoadScene("Map");
		}
	}
}