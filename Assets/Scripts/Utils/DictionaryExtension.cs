using System.Collections.Generic;

namespace Hmm3Clone.Utils {
	public static class DictionaryExtension {
		public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) {
			return dictionary.TryGetValue(key, out var res) ? res : default;
		}

		public static void IncrementAmount<TKey>(this Dictionary<TKey, int> dictionary, TKey key, int amount) {
			dictionary[key] = dictionary.GetOrDefault(key) + amount;
		}
	}
}