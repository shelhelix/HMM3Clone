using System;
using System.Collections.Generic;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.SpriteSetups;
using Hmm3Clone.State;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
	public class HiringWindow : GameComponent {
		[NotNull] public Button           HireButton;
		[NotNull] public Slider           UnitsAmount;
		[NotNull] public HiringUnitAvatar DefaultUnitAvatar;
		[NotNull] public HiringUnitAvatar AdvancedUnitAvatar;
		[NotNull] public TMP_Text         HeaderText;
		[NotNull] public TMP_Text         UnitsToRecruitText;

		[NotNullOrEmpty] public List<ResourcePriceView> ResourcePriceViews;

		[NotNull] public GameObject CantBuyScreen;

		UnitType _defaultUnit;
		UnitType _advancedUnit;

		UnitType _selectedUnitToHire;

		CityController   _cityController;
		UnitsController  _unitsController;
		UnitsSpriteSetup _unitsSpriteSetup;

		CityState _cityState;
		
		void Start() {
			_cityState       = ActiveData.Instance.GetData<CityState>();
			_cityController  = GameController.Instance.GetController<CityController>();
			_unitsController = GameController.Instance.GetController<UnitsController>();
			_unitsSpriteSetup = GameController.Instance.GetController<SpriteSetupController>()
											  .GetSpriteSetup<UnitsSpriteSetup>();
		}

		void OnDisable() {
			AdvancedUnitAvatar.gameObject.SetActive(true);
			AdvancedUnitAvatar.SelectUnitButton.onClick.RemoveAllListeners();
			DefaultUnitAvatar.SelectUnitButton.onClick.RemoveAllListeners();
		}

		public void Init(UnitType unitType) {
			_defaultUnit = unitType;

			var unitAdvancedForm = _unitsController.GetAdvancedUnitType(_defaultUnit);
			var canHireAdvanced  = _cityController.CanHireUnit(_cityState.CityName, unitAdvancedForm);
			AdvancedUnitAvatar.gameObject.SetActive(canHireAdvanced);
			if (canHireAdvanced) {
				AdvancedUnitAvatar.gameObject.SetActive(true);
				InitUnitAvatar(AdvancedUnitAvatar, unitAdvancedForm);
			}
			InitUnitAvatar(DefaultUnitAvatar, _defaultUnit);
			
			SwitchToUnit(canHireAdvanced ? _advancedUnit : _defaultUnit);

			HeaderText.text = $"Recruit {_defaultUnit}";
		
			RefreshHiringUi();
			
			HireButton.onClick.RemoveAllListeners();
			HireButton.onClick.AddListener(OnHireClick);
			
			CantBuyScreen.SetActive(false);
			gameObject.SetActive(true);
		}

		void RefreshHiringUi() {	
			var maxAvailableUnits = _cityController.GetMaxAvailableToBuyUnitsAmount(_cityState.CityName, _selectedUnitToHire);

			UnitsAmount.minValue     = Mathf.Min(1, maxAvailableUnits);
			UnitsAmount.maxValue     = maxAvailableUnits;
			UnitsAmount.value        = UnitsAmount.minValue;
			UnitsAmount.wholeNumbers = true;
			UnitsAmount.onValueChanged.AddListener(OnSliderValueChanged);
			OnSliderValueChanged(UnitsAmount.value);
		}

		void InitUnitAvatar(HiringUnitAvatar unitAvatar, UnitType unitType) {
			unitAvatar.Avatar.sprite = _unitsSpriteSetup.GetHireSprite(unitType);
			unitAvatar.SelectUnitButton.onClick.AddListener(() => SwitchToUnit(unitType));
		}
		
		void SwitchToUnit(UnitType unitType) {
			_selectedUnitToHire = unitType;
			RefreshHiringUi();
		}

		void OnHireClick() {
			gameObject.SetActive(false);
			if (_cityController.HasAvailableStackForUnits(_cityState.CityName, _selectedUnitToHire)) {
				_cityController.HireUnits(_cityState.CityName, _selectedUnitToHire, (int) UnitsAmount.value);
			} else {
				CantBuyScreen.SetActive(true);
			}
		}

		void OnSliderValueChanged(float value) {
			var unitsAmount = (int) value;
			var resultPrice = _cityController.GetUnitHiringPrice(_selectedUnitToHire, unitsAmount);
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