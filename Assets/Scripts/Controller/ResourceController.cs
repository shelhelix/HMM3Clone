using Hmm3Clone.State;

namespace Hmm3Clone.Controller {
	public class ResourceController : IController {
		ResourcesState _state;

		public ResourceController(ResourcesState state) {
			_state = state;
		} 
		
		public bool IsEnoughResource(Resource amount) {
			var resourceBalance = GetResource(amount.ResourceType);
			return (resourceBalance != null) && resourceBalance.Amount >= amount.Amount;
		}

		public int GetAmount(ResourceType resourceType) {
			return GetResource(resourceType).Amount;
		}

		Resource GetResource(ResourceType resourceType) {
			return _state.Resources.Find(x => x.ResourceType == resourceType);
		}
	}
}