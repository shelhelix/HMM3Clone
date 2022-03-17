using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.SpriteSetups;
using Hmm3Clone.State;
using TMPro;
using UnityEngine.UI;
using VContainer;

namespace Hmm3Clone.Behaviour {
	public class ResourcePriceView : GameComponent {
		[Inject]         SpriteSetupController _spriteSetupController;
		[NotNull] public Image                 ResourceIcon;
		[NotNull] public TMP_Text              AmountText;

		ResourcesSpriteSetup _resourcesSpriteSetup;
		
		void Start() {
			_resourcesSpriteSetup = _spriteSetupController.GetSpriteSetup<ResourcesSpriteSetup>();
		}

		public void SetPrice(Resource resource) {
			if (!_resourcesSpriteSetup) {
				Start();
			}
			ResourceIcon.sprite = _resourcesSpriteSetup.GetResourceSprite(resource);
			AmountText.text     = resource.Amount.ToString();
		}
	}
}