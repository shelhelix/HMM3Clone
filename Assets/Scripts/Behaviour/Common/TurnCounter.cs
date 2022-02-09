using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using TMPro;
using UnityEngine;

namespace Hmm3Clone.Behaviour {
    public class TurnCounter : MonoBehaviour {
        [NotNull] public TMP_Text Text;

        TurnController _turnController;
        
        void Start() {
            _turnController = GameController.Instance.GetController<TurnController>();
        }

        void Update() {
            Text.text = $"Day {_turnController.Turn.ToString()}";
        }
    }
}
