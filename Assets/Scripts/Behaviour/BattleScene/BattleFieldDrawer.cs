using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Manager;
using Hmm3Clone.Manager.Battle;
using Hmm3Clone.State;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

namespace Hmm3Clone.Behaviour.BattleScene {
	public class BattleFieldDrawer : GameComponent {
		
		[NotNull] public Tilemap UnitsTilemap;
		[NotNull] public Tile    UnitPlaceholderTile;

		[Inject] BattleManager _battleManager;

		[Inject]
		public void Init() {
			_battleManager.OnGridChanged += OnGridChanged;
			OnGridChanged(_battleManager.Grid);
		}

		void OnGridChanged(BattleUnitStack[,] grid) {
			UnitsTilemap.ClearAllTiles();
			for (var x = 0; x < grid.GetLength(0); x++) {
				for (var y = 0; y < grid.GetLength(1); y++) {
					var coords   = BattleManager.LowLeftCorner + new Vector3Int(x, y);
					var unitData = grid[x, y];
					if (unitData != null) {
						UnitsTilemap.SetTile(coords, UnitPlaceholderTile);
					}
				}
			}
		}
	}
}