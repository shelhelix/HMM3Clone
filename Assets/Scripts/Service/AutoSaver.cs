using Hmm3Clone.Behaviour.Common;
using Hmm3Clone.State;
using Hmm3Clone.Utils;
using UnityEngine;
using VContainer;

namespace Hmm3Clone.Service {
	public class AutoSaver : BaseInjectableComponent {
		[Inject] GameState _state;
		
		void OnApplicationQuit() {
			SaveUtils.SaveState(_state);
		}

		[RuntimeInitializeOnLoadMethod]
		static void CreateAutoSaver() {
			var go = new GameObject();
			go.AddComponent<AutoSaver>();
			DontDestroyOnLoad(go);
		}
	}
}