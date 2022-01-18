using System.IO;
using Hmm3Clone.State;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Hmm3Clone.Utils {
	public static class SaveUtils {
		static string SaveLocation => Path.Combine(Application.persistentDataPath, "gameState.h3save");

		[MenuItem("MyTools/Remove save")]
		public static void RemoveSave() {
			File.Delete(SaveLocation);
		}
		
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
	}
}