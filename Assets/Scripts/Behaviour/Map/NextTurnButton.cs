using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour.Common {
	[RequireComponent(typeof(Button))]
	public class NextTurnButton : BaseInjectableComponent {
		[NotNull] public Button Button;
		
		TurnController _turnController;

		void Reset() {
			Button = GetComponent<Button>();
		}
		
		void Start() {
			_turnController = GameController.Instance.GetController<TurnController>();
			Button.onClick.AddListener(_turnController.EndTurn);
		}
	}
}