using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using TMPro;
using UnityEngine;

namespace Hmm3Clone {
    public class ResourceView : MonoBehaviour {
        public ResourceType ResourceType;
        
        [NotNull] public TMP_Text AmountText;

        ResourceController _resourceController;

        void Start() {
            _resourceController = GameController.Instance.GetController<ResourceController>();
        }

        void Update() {
            AmountText.text = _resourceController.GetResourceAmount(ResourceType).ToString();
        }
    }
}
