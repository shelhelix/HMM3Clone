using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace Hmm3Clone.Behaviour.Map {
	[CreateAssetMenu]
	public class PathTile : TileBase {
		class ApplyRuleResult {
			public static ApplyRuleResult Failed => new ApplyRuleResult(false, 0);
			
			public readonly bool  IsOk;
			public readonly float Rotation;

			public ApplyRuleResult(bool isOk, float rotation) {
				IsOk     = isOk;
				Rotation = rotation;
			}
		}

		public Sprite               DefaultTile;
		public List<PathSpriteRule> Rules;
		
		// Common data between all tiles
		[HideInInspector]
		public List<Vector3Int> FullPath;
		
		public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData) {
			base.GetTileData(position, tilemap, ref tileData);
			tileData.sprite = DefaultTile;
			tileData.color = Color.red;
			foreach (var rule in Rules) {
				var res = TryApplyRule(rule, tilemap, position);
				if (!res.IsOk) {
					continue;
				}
				tileData.color  = Color.white;
				tileData.sprite = rule.Sprite;
				var transformMatrix = tileData.transform;
				transformMatrix.SetTRS(Vector3.zero, Quaternion.AngleAxis(res.Rotation, Vector3.forward), Vector3.one);
				tileData.transform = transformMatrix;
			}
		}

		ApplyRuleResult TryApplyRule(PathSpriteRule rule, ITilemap tilemap, Vector3Int tilePosition) {
			Assert.IsNotNull(FullPath);
			if (!rule.IsEnabled) {
				return ApplyRuleResult.Failed;
			}
			var pathCellIndex = FullPath.IndexOf(tilePosition);
			if (pathCellIndex == -1) {
				return ApplyRuleResult.Failed;
			}
			var rotations = (rule.TryRotateRule) ? 4 : 1;
			for (var i = 0; i < rotations; i++) {
				var angle      = i * 90;
				var quaternion = Quaternion.AngleAxis(angle, Vector3.forward);
				if (rule.HasStartPointNeighbour) {
					if (pathCellIndex == 0) {
						continue;
					}
					var currentStartPointOffset = quaternion * rule.StartPointOffset;
					var startPoint              = Vector3Int.RoundToInt(tilePosition + currentStartPointOffset);
					if (startPoint != FullPath[pathCellIndex - 1]) {
						continue;
					}
				} else {
					if (pathCellIndex != 0) {
						continue;
					}
				}
				
				if (rule.HasEndPointNeighbour) {
					if (pathCellIndex == FullPath.Count -1) {
						continue;
					}
					var currentEndPointOffset = quaternion * rule.EndPointOffset;
					var endPoint              = Vector3Int.RoundToInt(tilePosition + currentEndPointOffset);
					if (endPoint != FullPath[pathCellIndex + 1]) {
						continue;
					}
				} else {
					if (pathCellIndex != FullPath.Count -1) {
						continue;
					}
				}
				return new ApplyRuleResult(true, angle);
			}
			return ApplyRuleResult.Failed;
		}
	}
}