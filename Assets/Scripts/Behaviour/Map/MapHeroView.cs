using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.Gameplay;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour.Map {
	public class MapHeroView : GameComponent {
		const float MaxMovementPoints = 20f;
		
		[NotNull] public Image  Avatar;
		[NotNull] public Slider MovepointsBar;

		Hero _hero;
		
		public void Init(HeroController heroController, string heroName) {
			_hero         = heroController.GetHero(heroName);
			Avatar.sprite = heroController.GetHeroInfo(heroName)?.HeroAvatar;
			Update();
		}

		void Update() {
			if (_hero == null) {
				return;
			}
			MovepointsBar.value = _hero.MovementPoints / MaxMovementPoints;
		}
	}
}