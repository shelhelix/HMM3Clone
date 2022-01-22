using System;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.SpriteSetups;
using Hmm3Clone.State;
using TMPro;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
	public class ResourcePriceView : GameComponent {
		[NotNull] public Image    ResourceIcon;
		[NotNull] public TMP_Text AmountText;

		ResourcesSpriteSetup _resourcesSpriteSetup;
		
		void Start() {
			_resourcesSpriteSetup = GameController.Instance.GetController<SpriteSetupController>().ResourcesSpriteSetup;
		}

		public void SetPrice(Resource resource) {
			ResourceIcon.sprite = _resourcesSpriteSetup.GetResourceSprite(resource);
			AmountText.text     = resource.Amount.ToString();
		}
	}
}