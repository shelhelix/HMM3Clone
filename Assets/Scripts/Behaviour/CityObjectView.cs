using System.Collections.Generic;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.State;
using UnityEngine;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
	public class CityObjectView : GameComponent{
		[NotNull] public Button Button;
		[NotNull] public List<BuildingViewStage> Stages;
		
		public void Refresh() {
			var state = ActiveData.Instance.GetData<CityState>();
			var activeState = Stages.FindLast(x => state.IsErected(x.BuildingInfo.Name));
			Stages.ForEach(x => x.BuildingView.gameObject.SetActive(x == activeState));
			if (!activeState) {
				return;
			}
			Button.targetGraphic = activeState.BuildingView;
		}
	}
}