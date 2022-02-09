using System.Collections.Generic;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.Manager;
using UnityEngine;

namespace Hmm3Clone.Behaviour.Map {
	public class MapStarter : GameComponent {
		[NotNull] public RuntimeMapInfo MapInfo;
		
		[NotNull] public MapView MapView;
		
		MapManager     _mapManager;
		HeroController _heroController;

		List<Vector3Int> _lastPath;

		MapPathfinder _pathfinder;

		void OnDestroy() {
			MapView.Deinit();
		}

		void Start() {
			MapInfo.PrepareMap();
			_heroController = GameController.Instance.GetController<HeroController>();
			_pathfinder     = new MapPathfinder(_heroController, MapInfo);
			_mapManager     = new MapManager(_heroController, MapInfo);
			
			MapView.Init(_heroController, _mapManager, MapInfo);
		}
		
		void Update() {
			var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			worldPos.z = 0;
			var cellPos = MapInfo.Heroes.WorldToCell(worldPos);
			var hero    = _heroController.GetHero(HeroController.TestHeroName);
			var path    = _pathfinder.CreatePath(hero.Position, cellPos);

			MapView.DrawPath(path);
			
			if (Input.GetMouseButtonDown(0)) {
				_mapManager.MoveHero(hero.Name, cellPos);
			}
		}
	}
}