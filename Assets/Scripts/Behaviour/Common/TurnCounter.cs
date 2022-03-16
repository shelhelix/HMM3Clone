using GameComponentAttributes.Attributes;
using Hmm3Clone.Behaviour.Common;
using Hmm3Clone.Controller;
using TMPro;
using UnityEngine;
using VContainer;

namespace Hmm3Clone.Behaviour {
    public class TurnCounter : BaseInjectableComponent {
        [NotNull] public TMP_Text Text;

        [Inject] TurnController _turnController;

        void Update() {
            Text.text = $"Day {_turnController.Turn.ToString()}";
        }
    }
}
