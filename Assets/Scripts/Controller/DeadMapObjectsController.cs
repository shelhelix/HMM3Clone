using Hmm3Clone.State;
using UnityEngine;
using VContainer;

namespace Hmm3Clone.Controller {
	public class DeadMapObjectsController {
		[Inject] readonly MapState _mapState;

		public void RemoveObject(Vector3Int pos) {
			_mapState.RemovedObjectsFromMap.Add(pos);
		}

		public bool IsRemovedObject(Vector3Int pos) {
			return _mapState.RemovedObjectsFromMap.Contains(pos);
		}
	}
}