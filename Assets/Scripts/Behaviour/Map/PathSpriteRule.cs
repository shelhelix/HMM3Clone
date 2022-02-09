using System;
using NaughtyAttributes;
using UnityEngine;

namespace Hmm3Clone.Behaviour.Map {
	[Serializable]
	public class PathSpriteRule {
		public string Name;
		
		public bool IsEnabled = true;
		
		public bool HasStartPointNeighbour;
		[ShowIf(nameof(HasStartPointNeighbour))][AllowNesting]
		public Vector3Int StartPointOffset;
		
		public bool HasEndPointNeighbour;
		[ShowIf(nameof(HasEndPointNeighbour))][AllowNesting]
		public Vector3Int EndPointOffset;

		public bool TryRotateRule;

		[ShowAssetPreview()]
		public Sprite Sprite;
	}
}