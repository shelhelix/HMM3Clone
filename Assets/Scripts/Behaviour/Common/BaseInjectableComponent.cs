using System.Collections.Generic;
using GameComponentAttributes;

namespace Hmm3Clone.Behaviour.Common {
	public abstract class BaseInjectableComponent : GameComponent {
		public static List<BaseInjectableComponent> Components = new List<BaseInjectableComponent>();
		
		protected virtual void OnEnable() {
			Components.Add(this);
		}

		protected virtual void OnDisable() {
			Components.Remove(this);
		}

		protected virtual void OnDestroy() {
			Components.Remove(this);
		}
	}
}