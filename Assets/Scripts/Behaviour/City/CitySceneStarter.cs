using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Scopes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace Hmm3Clone.Behaviour {
	[DefaultExecutionOrder(-1000)]
	public class CitySceneStarter : GameComponent {
		[NotNull] public GameObject ScreenRoot;
		[NotNull] public Button     ReturnToMapButtom;

		[Inject] CityScope _scope;

		void Start() {
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