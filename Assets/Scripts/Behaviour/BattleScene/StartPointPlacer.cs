using System;
using System.Collections.Generic;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Hmm3Clone.Behaviour.BattleScene {
	[Serializable]
	public class StartPointsConfiguration {
		public List<Vector3Int> LeftArmyStartPoints;
		public List<Vector3Int> RightArmyStartPoints;
	}
	
	public class StartPointPlacer : GameComponent {
		public StartPointsConfiguration Configuration;

		// only for editor
		[NotNull] public Tilemap MetaObjectsTilemap;
		[NotNull] public Tile    StartPointTile;
		
		void Start() {
			MetaObjectsTilemap.gameObject.SetActive(Application.isPlaying);
		}

		protected new void OnValidate() {
			base.OnValidate();
			MetaObjectsTilemap.ClearAllTiles();
			Configuration.LeftArmyStartPoints.ForEach(x => MetaObjectsTilemap.SetTile(x, StartPointTile));
			Configuration.RightArmyStartPoints.ForEach(x => MetaObjectsTilemap.SetTile(x, StartPointTile));
		}
	}
}