using System.Collections.Generic;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Behaviour.Common;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using Hmm3Clone.Utils;
using TMPro;
using UnityEngine;
using VContainer;

namespace Hmm3Clone.Behaviour {
    public class CityOverallView : BaseInjectableComponent {
        [NotNull] public TMP_Text IncomeText;
        [NotNull] public TMP_Text CityName;

        [NotNullOrEmpty] public List<CityOverallViewUnitStack> UnitStacks;

        [Inject] CityController _cityController;
        [Inject] CityState      _activeCityState;
        
        
        void Start() {
            CityName.text = _activeCityState.CityName;
        }

        void Update() {
            var income = _cityController.GetCityIncome(_activeCityState.CityName);
            IncomeText.text = income.GetOrDefault(ResourceType.Gold).ToString();
            InitUnitsViews();
        }

        void InitUnitsViews() {
            var unitsAmount = _cityController.GetNotBoughtCityUnits(_activeCityState.CityName);
            var minCount = Mathf.Min(UnitStacks.Count, unitsAmount.Count);
            var index = 0; 
            foreach (var unitAmount in unitsAmount) {
                if (index >= minCount) {
                    return;
                }
                var view = UnitStacks[index];
                view.Init(unitAmount.Key, unitAmount.Value);
                index++;
            }
            for (; index < UnitStacks.Count; index++) {
                UnitStacks[index].SetActiveInternalObjects(false);
            }
        }
    }
}
