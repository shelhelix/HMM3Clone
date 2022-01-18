using GameComponentAttributes;
using Hmm3Clone.Utils;
using UnityEngine;

namespace Hmm3Clone.Service {
	public class AutoSaver : GameComponent {
		void OnApplicationQuit() {
			SaveUtils.SaveState(GameController.Instance.ActiveState);
		}

		[RuntimeInitializeOnLoadMethod]
		static void CreateAutoSaver() {
			var go = new GameObject();
			go.AddComponent<AutoSaver>();
			DontDestroyOnLoad(go);
		}
	}
}