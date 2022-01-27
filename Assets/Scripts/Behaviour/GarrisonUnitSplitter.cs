using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
	public class GarrisonUnitSplitter : GameComponent {
		public static GarrisonUnitSplitter Instance;
		
		[NotNull] public Toggle SplitToggle;

		CityGarrisonUnitStackView _selectedStackView;

		CityState      _state;
		CityController _cityController;
		
		void Start() {
			SplitToggle.onValueChanged.AddListener(SwitchSplit);
			_cityController = GameController.Instance.GetController<CityController>();
			_state          = ActiveData.Instance.GetData<CityState>();
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
				if (_cityController.TrySplitStacks(_state.CityName, _selectedStackView.Index, stackView.Index)) {
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