using UnityEngine;
using UnityEngine.Assertions;

namespace Hmm3Clone.Utils {
	public class ConfigLoader {
		public static T LoadConfig<T>() where T : ScriptableObject {
			var config = Resources.Load<T>($"Configs/{typeof(T).Name}");
			Assert.IsTrue(config);
			return config;
		}
	}
}