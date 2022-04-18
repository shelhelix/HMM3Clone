using System;
using System.Collections.Generic;
using System.Linq;
using Hmm3Clone.Behaviour.BattleScene;
using Hmm3Clone.Gameplay;
using Hmm3Clone.Scopes.Mediators;
using Hmm3Clone.State;
using UnityEngine;

namespace Hmm3Clone.Manager.Battle {
	public sealed class BattleManager {
		public const int BattlefieldWidth  = 15;
		public const int BattlefieldHeight = 11;

		public static Vector3Int LowLeftCorner = new Vector3Int(-7, -5, 0);
		
		public BattleUnitStack[,] Grid = new BattleUnitStack[BattlefieldWidth, BattlefieldHeight];

		public event Action<BattleUnitStack[,]> OnGridChanged;
		
		public BattleManager(MapToBattleSceneData startData, StartPointsConfiguration configuration) {
			PlaceSide(startData.LeftSide.Army, configuration.LeftArmyStartPoints);
			PlaceSide(startData.RightSide.Army, configuration.RightArmyStartPoints);
		}

		public List<Vector3Int> GetAvailableMovePoints(Vector3Int unitPosition) {
			// TODO: replace test results with real calculations
			return new List<Vector3Int> {Vector3Int.zero, Vector3Int.one, Vector3Int.right, Vector3Int.down, Vector3Int.up};
		}
		

		void PlaceSide(Army army, List<Vector3Int> startPoints) {
			var zippedCollection = army.Zip(startPoints, (stack, pos) => (stack, pos));
			foreach (var pair in zippedCollection) {
				var gridIndex = pair.pos - LowLeftCorner;
				if (!IsBetween(gridIndex.x, 0, Grid.GetLength(0)) || !IsBetween(gridIndex.y, 0, Grid.GetLength(1))) {
					Debug.LogError("grid index is out of bounds ");
					continue;
				}
				Grid[gridIndex.x, gridIndex.y] = new BattleUnitStack(pair.stack);
			}
			OnGridChanged?.Invoke(Grid);
		}

		bool IsBetween(int value, int min, int max) {
			return value >= min && value <= max;
		}
	}
}