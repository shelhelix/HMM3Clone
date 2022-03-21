using System;
using System.Collections.Generic;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using Hmm3Clone.Utils;
using TMPro;
using UnityEngine;
using VContainer;

namespace Hmm3Clone.Behaviour {
    public class CityOverallView : GameComponent {
        [NotNull] public TMP_Text IncomeText;
        [NotNull] public TMP_Text CityName;

        [NotNullOrEmpty] public List<CityOverallViewUnitStack> UnitStacks;

        [Inject] CityController _cityController;
        [Inject] CityState      _activeCityState;

        void Start() {
            _cityController.OnBuildingsChanged += RefreshView;
            CityName.text                      =  _activeCityState.CityName;
            RefreshView();
        }

        void OnDestroy() {
            _cityController.OnBuildingsChanged -= RefreshView;
        }

        void RefreshView() {
            var income = _cityController.GetCityIncome(_activeCityState.CityName);
            IncomeText.text = income.GetOrDefault(ResourceType.Gold).ToString();
            InitUnitsViews();
        }

        void InitUnitsViews() {
            var unitsAmount     = _cityController.GetNotBoughtCityUnits(_activeCityState.CityName);
            var availableUnitTypes = _cityController.GetUnitProductionAmount(_activeCityState.CityName).Keys;
            var minCount        = Mathf.Min(UnitStacks.Count, availableUnitTypes.Count);
            var index           = 0;
            foreach (var unit in availableUnitTypes) {
                if (index >= minCount) {
                    return;
                }
                var view   = UnitStacks[index];
                var amount = unitsAmount.GetOrDefault(unit);
                view.Init(unit, amount);
                index++;
            }
            for (; index < UnitStacks.Count; index++) {
                UnitStacks[index].SetActiveInternalObjects(false);
            }
        }
    }
}
