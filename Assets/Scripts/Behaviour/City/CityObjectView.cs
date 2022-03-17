using System.Collections.Generic;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.State;
using UnityEngine.UI;
using VContainer;

namespace Hmm3Clone.Behaviour {
	public class CityObjectView : GameComponent {
		[NotNull] public Button Button;
		[NotNull] public List<BuildingViewStage> Stages;
		
		[Inject] CityState _state;
		
		public void Refresh() {
			
			var activeState = Stages.FindLast(x => _state.IsErected(x.BuildingInfo.Name));
			Stages.ForEach(x => x.BuildingView.gameObject.SetActive(x == activeState));
			if (!activeState) {
				return;
			}
			Button.targetGraphic = activeState.BuildingView;
		}
	}
}