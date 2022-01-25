using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
	public class HiringUnitAvatar : GameComponent {
		[NotNull] public Image  Avatar;
		[NotNull] public Button SelectUnitButton;
	}
}