using System;
using System.Collections.Generic;

namespace Hmm3Clone.State {
	[Serializable]
	public class Resource {
		public ResourceType ResourceType;
		public int Amount;

		public Resource() { }

		public Resource(ResourceType type, int amount) {
			ResourceType = type;
			Amount = amount;
		}
	}
	
	[Serializable]
	public class ResourcesState {
		public List<Resource> Resources = new List<Resource>();

		public ResourcesState() {
			foreach ( var resourceType in (ResourceType[])Enum.GetValues(typeof(ResourceType))) {
				Resources.Add(new Resource(resourceType, 0));
			}
		}
	}
}