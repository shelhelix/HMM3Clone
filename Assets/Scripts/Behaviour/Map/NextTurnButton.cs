using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Hmm3Clone.Behaviour.Common {
	[RequireComponent(typeof(Button))]
	public class NextTurnButton : BaseInjectableComponent {
		[NotNull] public Button Button;
		
		[Inject] TurnController _turnController;

		void Reset() {
			Button = GetComponent<Button>();
		}
		
		void Start() {
			Button.onClick.AddListener(_turnController.EndTurn);
		}
	}
}