using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.SpriteSetups;
using Hmm3Clone.State;
using TMPro;
using UnityEngine.UI;
using VContainer;

namespace Hmm3Clone.Behaviour {
    public class ResourceView : GameComponent {
        [Inject] ResourceController    _resourceController;
        [Inject] SpriteSetupController _spriteSetupController;
        
        public ResourceType ResourceType;
        
        [NotNull] public TMP_Text AmountText;
        [NotNull] public Image    ResourceImage;

        void Start() {
            ResourceImage.sprite = _spriteSetupController.GetSpriteSetup<ResourcesSpriteSetup>()
                                                         .GetResourceSprite(ResourceType);
        }

        void Update() {
            AmountText.text = _resourceController.GetResourceAmount(ResourceType).ToString();
        }
    }
}
