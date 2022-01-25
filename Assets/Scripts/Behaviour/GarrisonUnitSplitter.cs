using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.Gameplay;
using Hmm3Clone.State;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
	public class GarrisonUnitSplitter : GameComponent {
		public static GarrisonUnitSplitter Instance;
		
		[NotNull] public Toggle SplitToggle;

		CityGarrisonUnitStackView _selectedStackView;

		Army           _garrison;
		CityState      _state;
		CityController _cityController;
		
		void Start() {
			SplitToggle.onValueChanged.AddListener(SwitchSplit);
			_cityController = GameController.Instance.GetController<CityController>();
			_state          = ActiveData.Instance.GetData<CityState>();
			_garrison       = _cityController.GetCityGarrison(_state.CityName);
			Instance        = this;
		}

		void OnDestroy() {
			if (Instance == this) {
				Instance = null;
			}
		}

		public void OnUnitStackSelected(CityGarrisonUnitStackView stackView) {
			if (!SplitToggle.isOn) {
				return;
			}
			if (!_selectedStackView) {
				_selectedStackView = stackView;
			} else {
				if (stackView == _selectedStackView) {
					return;
				}
				if (_garrison.IsStackEmpty(stackView.StackIndex)) {	
					_cityController.SplitStacks(_state.CityName, _selectedStackView.StackIndex, stackView.StackIndex);
					SplitToggle.isOn = false;
				} else {
					_selectedStackView = stackView;
				}
			}
		}


		void SwitchSplit(bool newValue) {
			if (!newValue) {
				_selectedStackView = null;
			}
		}
	}
}