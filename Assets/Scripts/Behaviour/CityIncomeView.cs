using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using TMPro;
using UnityEngine;

namespace Hmm3Clone.Behaviour {
    public class CityIncomeView : MonoBehaviour {
        [NotNull] public TMP_Text IncomeText;

        CityController _cityController;
        CityState _activeCityState;
        
        
        void Start() {
            _cityController = GameController.Instance.GetController<CityController>();
            _activeCityState = ActiveData.Instance.GetData<CityState>();
            
        }

        void Update() {
            var income = _cityController.GetCityIncome(_activeCityState.CityName);
            if (income.TryGetValue(ResourceType.Gold, out var amount)) {
                IncomeText.text = amount.ToString();
            }
        }
    }
}
