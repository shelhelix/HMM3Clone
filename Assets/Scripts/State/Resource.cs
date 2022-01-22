using System;

namespace Hmm3Clone.State {
	[Serializable]
	public struct Resource {
		public ResourceType ResourceType;
		public int          Amount;
		
		public Resource(ResourceType type, int amount) {
			ResourceType = type;
			Amount       = amount;
		}

		public static implicit operator ResourceType(Resource resource) => resource.ResourceType;
	}
}