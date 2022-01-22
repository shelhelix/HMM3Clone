using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
    public class ResourceView : MonoBehaviour {
        public ResourceType ResourceType;
        
        [NotNull] public TMP_Text AmountText;
        [NotNull] public Image    ResourceImage;
        
        ResourceController _resourceController;

        void Start() {
            _resourceController = GameController.Instance.GetController<ResourceController>();
            ResourceImage.sprite = GameController.Instance.GetController<SpriteSetupController>().ResourcesSpriteSetup
                                                 .GetResourceSprite(ResourceType);
        }

        void Update() {
            AmountText.text = _resourceController.GetResourceAmount(ResourceType).ToString();
        }
    }
}
