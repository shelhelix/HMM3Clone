using GameComponentAttributes;
using Hmm3Clone.State;
using Hmm3Clone.Utils;
using UnityEngine;

namespace Hmm3Clone.Service {
	public class AutoSaver : GameComponent {
		public static AutoSaver Instance {
			get {
				if (!_instance) {
					CreateAutoSaver();
				}

				return _instance;
			}
		}

		static AutoSaver _instance;
		
		public GameState State;
		
		void OnApplicationQuit() {
			SaveUtils.SaveState(State);
		}

		static void CreateAutoSaver() {
			var go = new GameObject();
			_instance = go.AddComponent<AutoSaver>();
			DontDestroyOnLoad(go);
		}
	}
}