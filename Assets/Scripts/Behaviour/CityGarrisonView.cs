using System;
using System.Collections.Generic;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using UnityEngine.Assertions;

namespace Hmm3Clone.Behaviour {
	public class CityGarrisonView : GameComponent {
		[NotNull] public List<CityGarrisonUnitStackView> CityUnitStacks;

		CityState      _cityState;
		CityController _cityController;

		void OnDestroy() {
			_cityController.OnGarrisonChanged -= Refresh;
		}

		void Start() {
			_cityController                   =  GameController.Instance.GetController<CityController>();
			_cityState                        =  ActiveData.Instance.GetData<CityState>();
			_cityController.OnGarrisonChanged += Refresh;
			Refresh();
		}
		
		public void Refresh() {
			var garrisonUnits = _cityController.GetCityGarrison(_cityState.CityName); 
			Assert.IsTrue(CityUnitStacks.Count >= garrisonUnits.Count);
			for (var viewIndex = 0; viewIndex < CityUnitStacks.Count; viewIndex++) {
				if (viewIndex < garrisonUnits.Count) {
					var unit = garrisonUnits[viewIndex];
					CityUnitStacks[viewIndex].InitView(unit.Type, unit.Amount);		
				}
				CityUnitStacks[viewIndex].SetActive(viewIndex < garrisonUnits.Count);
			}
		}
	}
}