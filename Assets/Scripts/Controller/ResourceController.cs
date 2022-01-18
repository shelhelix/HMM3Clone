using Hmm3Clone.State;
using UnityEngine.Assertions;

namespace Hmm3Clone.Controller {
	public class ResourceController : IController {
		ResourcesState _state;

		public ResourceController(ResourcesState state) {
			_state = state;
		} 
		
		public bool IsEnoughResource(Resource amount) {
			var resourceBalance = GetResource(amount.ResourceType);
			Assert.IsNotNull(resourceBalance);
			return resourceBalance.Amount >= amount.Amount;
		}

		public int GetResourceAmount(ResourceType resourceType) {
			return GetResource(resourceType).Amount;
		}

		public void AddResource(Resource resource) {
			var resourceBalance = GetResource(resource.ResourceType);
			Assert.IsNotNull(resourceBalance);
			resourceBalance.Amount += resource.Amount;
		}

		Resource GetResource(ResourceType resourceType) {
			var resourceState = _state.Resources.Find(x => x.ResourceType == resourceType);
			if (resourceState == null) {
				resourceState = new Resource(resourceType, 0);
				_state.Resources.Add(resourceState);
			}
			return resourceState;
		}
	}
}