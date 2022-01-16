using System;
using System.Collections.Generic;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;

namespace Hmm3Clone.Behaviour {
	public class CityView : GameComponent {
		[NotNullOrEmpty] public List<CityObjectView> CityElements;

		
		
		void Start() {
			RefreshAll();
		}
		
		public void RefreshAll() {
			CityElements.ForEach(x => x.Refresh());
		}
	}
}