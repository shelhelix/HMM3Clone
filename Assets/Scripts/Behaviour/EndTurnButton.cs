using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
	public class EndTurnButton : GameComponent {
		[NotNull] public Button Button;
		
		protected override void Awake() {
			base.Awake();
			Button.onClick.AddListener(GameController.Instance.GetController<TurnController>().EndTurn);
		}
		
	}
}