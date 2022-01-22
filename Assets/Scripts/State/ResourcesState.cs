using System;
using System.Collections.Generic;

namespace Hmm3Clone.State {
	[Serializable]
	public class ResourcesState {
		public Dictionary<ResourceType, int> Resources = new Dictionary<ResourceType, int>();
	}
}