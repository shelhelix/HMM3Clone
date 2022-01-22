using System.Collections.Generic;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using Hmm3Clone.Utils;
using TMPro;
using UnityEngine;

namespace Hmm3Clone.Behaviour {
    public class CityOverallView : MonoBehaviour {
        [NotNull] public TMP_Text IncomeText;
        [NotNull] public TMP_Text CityName;

        [NotNullOrEmpty] public List<CityOverallViewUnitStack> UnitStacks;
        
        CityController _cityController;
        CityState _activeCityState;
        
        void Start() {
            _cityController = GameController.Instance.GetController<CityController>();
            _activeCityState = ActiveData.Instance.GetData<CityState>();

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
