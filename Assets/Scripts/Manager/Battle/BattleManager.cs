using System.Collections.Generic;
using Hmm3Clone.Behaviour.BattleScene;
using Hmm3Clone.Gameplay;
using Hmm3Clone.Scopes.Mediators;
using UnityEngine;

namespace Hmm3Clone.Manager.Battle {
	public sealed class BattleManager {
		
		public BattleManager(BattleSideInfo leftSide, BattleSideInfo rightSide, StartPointsConfiguration configuration) {
			PlaceSide(leftSide.Army, configuration.LeftArmyStartPoints);
			PlaceSide(rightSide.Army, configuration.RightArmyStartPoints);
		}

		public List<Vector3Int> GetAvailableMovePoints(Vector3Int unitPosition) {
			// TODO: replace test results with real calculations
			return new List<Vector3Int> {Vector3Int.zero, Vector3Int.one, Vector3Int.right, Vector3Int.down, Vector3Int.up};
		}
		

		void PlaceSide(Army leftSideArmy, List<Vector3Int> leftArmyStartPoints) {
			// TODO: place units on battle grid
		}
	}
}