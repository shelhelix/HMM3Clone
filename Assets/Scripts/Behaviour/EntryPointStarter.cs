using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Hmm3Clone.Behaviour {
	public class EntryPointStarter : IStartable {
		public void Start() {
			SceneManager.LoadScene("Map");
		}
	}
}