using Hmm3Clone.State;

namespace Hmm3Clone {
	public class ResourceController : IController {
		ResourcesState _state;

		public ResourceController(ResourcesState state) {
			_state = state;
		} 
		
		public bool IsEnoughResource(Resource amount) {
			var resourceBalance = _state.Resources.Find(x => x.ResourceType == amount.ResourceType);
			return (resourceBalance != null) && resourceBalance.Amount >= amount.Amount;
		} 
	}
}