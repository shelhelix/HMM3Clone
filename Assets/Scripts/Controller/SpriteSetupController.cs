using System.Collections.Generic;
using Hmm3Clone.SpriteSetups;
using Hmm3Clone.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hmm3Clone.Controller {
	public class SpriteSetupController : IController {
		readonly List<ScriptableObject> _spriteSetups;

		public SpriteSetupController() {
			_spriteSetups = new List<ScriptableObject> {
				ConfigLoader.LoadConfig<ResourcesSpriteSetup>(),
				ConfigLoader.LoadConfig<UnitsSpriteSetup>(),
			};
		}

		public T GetSpriteSetup<T>() where T : ScriptableObject {
			var ss = _spriteSetups.Find(x => x is T);
			Assert.IsTrue(ss);
			return ss as T;
		}
	}
}