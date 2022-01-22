using Hmm3Clone.SpriteSetups;
using Hmm3Clone.Utils;

namespace Hmm3Clone.Controller {
	public class SpriteSetupController : IController {
		public ResourcesSpriteSetup ResourcesSpriteSetup;

		public SpriteSetupController() {
			ResourcesSpriteSetup = ConfigLoader.LoadConfig<ResourcesSpriteSetup>();
		}
	}
}