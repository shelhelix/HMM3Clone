using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Behaviour.Common;
using Hmm3Clone.Controller;
using Hmm3Clone.Gameplay;
using Hmm3Clone.Manager;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour.Map {
	public class MapHeroView : BaseInjectableComponent {
		const float MaxMovementPoints = 20f;
		
		[NotNull] public Image  Avatar;
		[NotNull] public Button AvatarButton;
		[NotNull] public Image  SelectionOutline;
		[NotNull] public Slider MovepointsBar;

		Hero _hero;

		MapManager _mapManager;
		
		public void Init(MapManager mapManager, HeroController heroController, string heroName) {
			_mapManager   = mapManager;
			_hero         = heroController.GetHero(heroName);
			Assert.IsNotNull(_hero);
			Avatar.sprite = heroController.GetHeroInfo(heroName).HeroAvatar;
			AvatarButton.onClick.AddListener(SelectHero);
			
			_mapManager.SelectedHeroName.OnValueChanged += OnSelectionHeroChanged;
			
			OnSelectionHeroChanged(mapManager.SelectedHeroName);
			Update();
		}

		public void Deinit() {
			if (_mapManager != null) {
				_mapManager.SelectedHeroName.OnValueChanged -= OnSelectionHeroChanged;
			}
			AvatarButton.onClick.RemoveListener(SelectHero);
		}


		void SelectHero() {
			_mapManager.SelectHero(_hero.Name);
		}
		
		void OnSelectionHeroChanged(string newSelectedHero) {
			SelectionOutline.gameObject.SetActive(newSelectedHero == _hero.Name);
		}

		void Update() {
			if (_hero == null) {
				return;
			}
			MovepointsBar.value = _hero.MovementPoints / MaxMovementPoints;
		}
	}
}