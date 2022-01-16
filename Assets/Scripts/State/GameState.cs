using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Hmm3Clone.State {
	[Serializable]
	public class GameState {
		public static string SaveLocation => Path.Combine(Application.persistentDataPath, "gameState.h3save");
		
		public static GameState LoadState() {
			if (!File.Exists(SaveLocation)) {
				return new GameState();
			}
			var file = File.ReadAllText(SaveLocation);
			Debug.Log("state loaded");
			return JsonConvert.DeserializeObject<GameState>(file);
		}

		public static void SaveState(GameState state) {
			var json = JsonConvert.SerializeObject(state);
			File.WriteAllText(SaveLocation, json);
			Debug.Log("state saved");
		}
		
		public ResourcesState ResourcesState = new ResourcesState();
	}
}