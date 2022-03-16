using System.Collections.Generic;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Behaviour.Common;
using Hmm3Clone.Controller;
using Hmm3Clone.Scopes;
using Hmm3Clone.State;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace Hmm3Clone.Behaviour {
	public class CitySceneStarter : BaseInjectableComponent {
		[NotNull] public GameObject ScreenRoot;
		[NotNull] public Button     ReturnToMapButtom;

		[Inject] CityController _cityController;
		[Inject] CityScope      _scope;


		[Inject]
		void Init() {
			foreach (Transform child in ScreenRoot.transform) {
				child.gameObject.SetActive(false);
			}
			InitButtons();
			var copy = new List<BaseInjectableComponent>(Components);
			copy.ForEach(x => _scope.Container.Inject(x));
			
		}

		void InitButtons() {
			ReturnToMapButtom.onClick.AddListener(ReturnToMap);
		}

		void ReturnToMap() {
			SceneManager.LoadScene("Map");
		}
	}
}