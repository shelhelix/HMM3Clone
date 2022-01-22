using System.Collections.Generic;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
	public class HiringWindow : GameComponent {
		[NotNull] public Button HireButton;
		[NotNull] public Slider UnitsAmount;

		[NotNull] public TMP_Text HeaderText;
		[NotNull] public TMP_Text UnitsToRecruitText;

		[NotNullOrEmpty] public List<ResourcePriceView> ResourcePriceViews;

		[NotNull] public GameObject CantBuyScreen;

		UnitType _unitToHire;

		CityController _cityController;

		CityState _cityState;
		
		void Start() {
			_cityState      = ActiveData.Instance.GetData<CityState>();
			_cityController = GameController.Instance.GetController<CityController>();
		}

		public void Init(UnitType unitType) {
			_unitToHire = unitType;
			var maxAvailableUnits  = _cityController.GetMaxAvailableToBuyUnitsAmount(_cityState.CityName, _unitToHire);

			UnitsAmount.minValue     = Mathf.Min(1, maxAvailableUnits);
			UnitsAmount.maxValue     = maxAvailableUnits;
			UnitsAmount.value        = UnitsAmount.minValue;
			UnitsAmount.wholeNumbers = true;
			UnitsAmount.onValueChanged.AddListener(OnSliderValueChanged);
			OnSliderValueChanged(UnitsAmount.value);

			HeaderText.text = $"Recruit {_unitToHire}";
			
			HireButton.onClick.RemoveAllListeners();
			HireButton.onClick.AddListener(OnHireClick);
			CantBuyScreen.SetActive(false);
			gameObject.SetActive(true);
		}

		void OnHireClick() {
			gameObject.SetActive(false);
			if (_cityController.HasStackForUnits(_cityState.CityName, _unitToHire)) {
				_cityController.HireUnits(_cityState.CityName, _unitToHire, (int) UnitsAmount.value);
			} else {
				CantBuyScreen.SetActive(true);
			}
		}

		void OnSliderValueChanged(float value) {
			var unitsAmount = (int) value;
			var resultPrice = _cityController.GetUnitHiringPrice(_unitToHire, unitsAmount);
			Assert.IsTrue(ResourcePriceViews.Count >= resultPrice.Count);
			for (var index = 0; index < ResourcePriceViews.Count; index++) {
				var view = ResourcePriceViews[index];
				view.gameObject.SetActive(index < resultPrice.Count);
				if (index < resultPrice.Count) {
					view.SetPrice(resultPrice[index]);
				}
			}
			UnitsToRecruitText.text = unitsAmount.ToString();
		}
	}
}