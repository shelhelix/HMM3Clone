using Hmm3Clone.State;
using UnityEngine;

namespace Hmm3Clone.Gameplay {
	public class Hero {
		HeroState _state;
		
		public readonly Army Army;
		
		public string Name {
			get => _state.HeroName;
			set => _state.HeroName = value;
		}
		
		public Vector3Int Position {
			get => _state.MapPosition;
			set => _state.MapPosition = value;
		}

		public Vector3Int PathEndPoint {
			get => _state.PathEndPoint;
			set => _state.PathEndPoint = value;
		}
		
		public int MovementPoints {
			get => _state.LeftMovementPoints;
			set => _state.LeftMovementPoints = value;
		}

		public Hero(HeroState state) {
			_state = state;
			Army   = new Army(_state.Stacks);
		}
	}
}