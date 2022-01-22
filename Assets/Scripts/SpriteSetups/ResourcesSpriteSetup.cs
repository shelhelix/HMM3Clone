using System.Collections.Generic;
using Hmm3Clone.State;
using UnityEngine;

namespace Hmm3Clone.SpriteSetups {
	[CreateAssetMenu]
	public class ResourcesSpriteSetup : ScriptableObject {
		public List<ResourceSpriteContainer> ResourceSprites;

		public Sprite GetResourceSprite(ResourceType resourceType) {
			return ResourceSprites.Find(x => x.ResourceType == resourceType)?.ResourceSprite;
		}
	}
}