using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using TMPro;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
	public class BuildingViewStage : GameComponent {
		[NotNull(checkPrefab: false)] public BuildingInfo BuildingInfo;
		[NotNull] public Image BuildingView;
		[NotNull] public TMP_Text BuildingName;
		void Start() {
			BuildingName.text = BuildingInfo.Name;
		}
	}
}