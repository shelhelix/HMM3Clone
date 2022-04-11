using Hmm3Clone.Behaviour.Map;
using Hmm3Clone.Config.Map;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;

namespace Hmm3Clone.Controller {
	public class NeutralArmyController {
		[Inject] DeadMapObjectsController _deadMapObjectsController;
		[Inject] RuntimeMapInfo           _mapInfo;

		public bool IsNeutralArmyCell(Vector3Int pos) {
			return _mapInfo.IsNeutralArmyCell(pos) &&
				   !_deadMapObjectsController.IsRemovedObject(pos);
		}

		[CanBeNull]
		public MapNeutralArmyInfo GetNeutralArmyOnCell(Vector3Int pos) {
			if (!IsNeutralArmyCell(pos)) {
				return null;
			}
			return _mapInfo.GetNeutralArmyInfo(pos);
		}

		public void RemoveNeutralArmy(Vector3Int pos) {
			_deadMapObjectsController.RemoveObject(pos);
		}
	}
}