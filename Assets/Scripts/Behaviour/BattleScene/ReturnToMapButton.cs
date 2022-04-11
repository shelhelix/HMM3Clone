using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace Hmm3Clone.Behaviour.BattleScene {
	[RequireComponent(typeof(Button))]
	public class ReturnToMapButton : GameComponent {
		[NotNull] public Button Button;
		
		void Reset() {
			Button = GetComponent<Button>();
		}
		
		void Start() {
			Button.onClick.AddListener(ReturnToMap);
		}

		void ReturnToMap() {
			SceneManager.LoadScene("Map");
		}
	}
}