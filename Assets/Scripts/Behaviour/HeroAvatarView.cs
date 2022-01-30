using GameComponentAttributes.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
	public class HeroAvatarView : BaseDraggableUiItem<HeroAvatarView, HeroDragManager> {
		[NotNull] public Image     Avatar;
		
		[HideInInspector]
		public ArmySource Source;

		public bool IsActive => MovableRoot.gameObject.activeSelf;
		
		public void InitAvatar(Sprite avatar, ArmySource armySource) {
			Avatar.sprite = avatar;
			Source        = armySource;
		}

		public void SetActive(bool isActive) {
			MovableRoot.gameObject.SetActive(isActive);
		}
	}
}