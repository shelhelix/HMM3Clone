using Hmm3Clone.State;
using UnityEngine;

namespace Hmm3Clone.Gameplay {
	public class Hero {
		HeroState _state;

		public readonly Army Army;

		public Vector3Int Position {
			get => _state.MapPosition;
			set => _state.MapPosition = value;
		}

		public Hero(HeroState state) {
			_state = state;
			Army   = new Army(_state.Stacks);
		}
	}
}