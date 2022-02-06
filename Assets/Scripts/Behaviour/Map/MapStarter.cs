
using System;
using System.Collections.Generic;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Config.Map;
using Hmm3Clone.Controller;
using Hmm3Clone.Gameplay;
using Hmm3Clone.Manager;
using NesScripts.Controls.PathFind;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Hmm3Clone.Behaviour.Map {
	public class MapStarter : GameComponent{
		[NotNull] public Tilemap HeroLayer;
		[NotNull] public Tilemap ObjectsLayer;
		[NotNull] public Tilemap TerrainLayer;
		
		[NotNull] public MapView MapView;
		[NotNull] public MapInfo MapInfo;
		
		MapManager     _mapManager;
		HeroController _heroController;

		List<Vector3Int> _lastPath;

		void OnDestroy() {
			MapView.Deinit();
		}

		void Start() {
			TerrainLayer.CompressBounds();
			_heroController = GameController.Instance.GetController<HeroController>();
			_mapManager     = new MapManager(_heroController, TerrainLayer.cellBounds, MapInfo);
			
			MapView.Init(_heroController, _mapManager, ObjectsLayer, HeroLayer, MapInfo);
		}
		
		void Update() {
			var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			worldPos.z = 0;
			var cellPos = HeroLayer.WorldToCell(worldPos);
			var hero    = _heroController.GetHero(HeroController.TestHeroName);
			var path    = _mapManager.CreatePath(hero.Position, cellPos);

			_lastPath?.ForEach(x => TerrainLayer.SetColor(x, Color.white));
			path?.ForEach(x => TerrainLayer.SetColor(x, Color.yellow));
			_lastPath = path;

			if (Input.GetMouseButtonDown(0)) {
				_mapManager.MoveHero(hero.Name, cellPos);
			}
		}
	}
}