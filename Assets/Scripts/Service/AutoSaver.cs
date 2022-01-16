using GameComponentAttributes;
using Hmm3Clone.State;
using UnityEngine;

namespace Hmm3Clone {
	public class AutoSaver : GameComponent {
		void OnApplicationQuit() {
			GameState.SaveState(GameController.Instance.ActiveState);
		}

		[RuntimeInitializeOnLoadMethod]
		static void CreateAutoSaver() {
			var go = new GameObject();
			go.AddComponent<AutoSaver>();
			DontDestroyOnLoad(go);
		}
	}
}