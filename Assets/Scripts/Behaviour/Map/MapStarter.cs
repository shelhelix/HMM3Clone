using System.Collections.Generic;
using GameComponentAttributes;
using Hmm3Clone.Behaviour.Common;
using Hmm3Clone.Scopes;

namespace Hmm3Clone.Behaviour.Map {
	public class MapStarter : GameComponent {
		protected void Start() {
			var copy = new List<BaseInjectableComponent>(BaseInjectableComponent.Components);
			copy.ForEach(x => MapScope.Instance.Container.Inject(x));
		}
	}
}