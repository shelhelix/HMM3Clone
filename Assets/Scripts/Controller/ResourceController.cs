using Hmm3Clone.State;
using Hmm3Clone.Utils;
using UnityEngine.Assertions;

namespace Hmm3Clone.Controller {
	public class ResourceController : IController {
		readonly ResourcesState _state;

		public ResourceController(ResourcesState state) {
			_state = state;
		} 
		
		public bool IsEnoughResource(Resource resource) {
			Assert.AreNotEqual(resource.ResourceType, ResourceType.Invalid);
			return GetResourceAmount(resource.ResourceType) >= resource.Amount;
		}

		public int GetResourceAmount(ResourceType type) {
			return _state.Resources.GetOrDefault(type);
		}

		public void SubResources(Resource resource) {
			Assert.AreNotEqual(resource.ResourceType, ResourceType.Invalid);
			var currentAmount = GetResourceAmount(resource.ResourceType);
			Assert.IsTrue(currentAmount >= resource.Amount);
			_state.Resources.IncrementAmount(resource.ResourceType, -resource.Amount);
		}
		
		public void AddResource(Resource resource) {
			Assert.AreNotEqual(resource.ResourceType, ResourceType.Invalid);
			_state.Resources.IncrementAmount(resource.ResourceType, resource.Amount);
		}
	}
}