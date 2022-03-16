using System;
using System.Collections.Generic;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.Gameplay;
using Hmm3Clone.State;
using UnityEngine.Assertions;
using VContainer;

namespace Hmm3Clone.Behaviour {
	public enum ArmySource {
		Garrison,
		GuestHero
	}
	
	public class CityGarrisonView : GameComponent {
		[NotNull] public HeroAvatarView GarrisonHeroAvatarView;
		[NotNull] public List<CityGarrisonUnitStackView> CityUnitStacks;
		
		[NotNull] public HeroAvatarView GuestHeroAvatarView;
		[NotNull] public List<CityGarrisonUnitStackView> GuestHeroUnitStacks;

		[Inject] CityState      _cityState;
		[Inject] CityController _cityController;
		[Inject] HeroController _heroController;

		Army _emptyArmy = new Army(new UnitStack[7]);

		void OnDestroy() {
			_cityController.OnArmyChanged -= Refresh;
		}

		void Start() {
			_cityController.OnArmyChanged += Refresh;
			Refresh();
		}

		void Refresh() {
			var garrisonHeroName = _cityController.GetGarrisonHeroName(_cityState.CityName);
			InitHeroAvatar(ArmySource.Garrison, garrisonHeroName, GarrisonHeroAvatarView);
			var garrisonUnits = _cityController.GetCityGarrison(_cityState.CityName);
			InitViews(ArmySource.Garrison, garrisonUnits, CityUnitStacks);

			var guestHeroName = _cityController.GetGuestHeroName(_cityState.CityName);
			InitHeroAvatar(ArmySource.GuestHero, guestHeroName, GuestHeroAvatarView);
			var guestHeroArmy = string.IsNullOrEmpty(guestHeroName)
									? _emptyArmy
									: _heroController.GetHero(guestHeroName).Army;
			InitViews(ArmySource.GuestHero, guestHeroArmy, GuestHeroUnitStacks);
		}

		void InitHeroAvatar(ArmySource source, string heroName, HeroAvatarView avatarView) {
			avatarView.SetActive(!string.IsNullOrEmpty(heroName));
			if (!avatarView.IsActive) {
				return;
			}
			var heroInfo = _heroController.GetHeroInfo(heroName);
			avatarView.InitAvatar(heroInfo.HeroAvatar, source);
		}
		
		void InitViews(ArmySource armySource, Army army, List<CityGarrisonUnitStackView> armyViewRow ) { 
			Assert.IsTrue(armyViewRow.Count >= army.ArmyLenght);
			for (var viewIndex = 0; viewIndex < armyViewRow.Count; viewIndex++) {
				var view = armyViewRow[viewIndex];
				view.InitCommonView(new CityUnitStackIndex(armySource, viewIndex));
				view.SetActive(viewIndex < army.ArmyLenght && !army.IsStackEmpty(viewIndex));
				if (view.IsActive) {
					var unit = army.GetStack(viewIndex);
					view.InitView(unit);
				}
			}
		}
	}
}