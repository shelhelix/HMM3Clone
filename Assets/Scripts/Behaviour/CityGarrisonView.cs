using System;
using System.Collections.Generic;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.Gameplay;
using Hmm3Clone.State;
using UnityEngine.Assertions;

namespace Hmm3Clone.Behaviour {
	public enum ArmySource {
		Garrison,
		GuestHero
	}
	
	public class CityGarrisonView : GameComponent {
		[NotNull] public List<CityGarrisonUnitStackView> CityUnitStacks;
		[NotNull] public List<CityGarrisonUnitStackView> GuestHeroUnitStacks;
		
		CityState      _cityState;
		CityController _cityController;
		HeroController _heroController;

		void OnDestroy() {
			_cityController.OnArmyChanged -= Refresh;
		}

		void Start() {
			_cityController                   =  GameController.Instance.GetController<CityController>();
			_heroController                   =  GameController.Instance.GetController<HeroController>();
			_cityState                        =  ActiveData.Instance.GetData<CityState>();
			_cityController.OnArmyChanged += Refresh;
			Refresh();
		}

		void Refresh() {
			var garrisonUnits = _cityController.GetCityGarrison(_cityState.CityName);
			var guestHeroName = _cityController.GetGuestHeroName(_cityState.CityName);
			var guestHeroArmy = _heroController.GetHero(guestHeroName).Army;
			InitViews(ArmySource.Garrison, garrisonUnits, CityUnitStacks);
			InitViews(ArmySource.GuestHero, guestHeroArmy, GuestHeroUnitStacks);
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