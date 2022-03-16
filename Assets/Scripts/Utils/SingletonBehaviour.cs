using UnityEngine;

namespace Hmm3Clone.Utils {
	public class SingletonBehaviour<T> where T : MonoBehaviour {
		static T _instance;

		public static T Instance {
			get {
				if (_instance == null) {
					_instance = Object.FindObjectOfType<T>();
					Object.DontDestroyOnLoad(_instance);
				}
				return _instance;
			}
		}
	}
}