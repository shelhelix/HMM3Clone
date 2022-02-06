using System;
using System.Collections.Generic;
using System.Linq;
using Hmm3Clone.Config.Map;
using Hmm3Clone.Controller;
using NesScripts.Controls.PathFind;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

using PathGrid = NesScripts.Controls.PathFind.Grid;

namespace Hmm3Clone.Manager {
	public class MapManager {
		readonly HeroController _heroController;
		readonly PathGrid       _grid;
		readonly MapInfo        _mapInfo;
		
		public event Action MapChanged;

		BoundsInt _mapSizesBounds;
		
		public MapManager(HeroController heroController, BoundsInt mapSizes, MapInfo mapInfo) {
			_heroController = heroController;
			_mapInfo        = mapInfo;
			_mapSizesBounds = mapSizes;
			_grid           = CreatePathfindingGrid();
		}


		public List<Vector3Int> CreatePath(Vector3Int start, Vector3Int end) {
			if (!_mapSizesBounds.Contains(start) || !_mapSizesBounds.Contains(end)) {	
				return null;
			}
			var cellStart = ConvertTilemapToGridCoords(start);
			var cellEnd   = ConvertTilemapToGridCoords(end);
			return Pathfinding.FindPath(_grid, cellStart, cellEnd).Select(x => new Vector3Int(x.x, x.y, 0) + _mapSizesBounds.min).ToList();
		}

		public void MoveHero(string heroName, Vector3Int endPoint) {
			var hero = _heroController.GetHero(heroName);
			var path = CreatePath(hero.Position, endPoint);
			if (path == null || path.Count == 0) {
				Debug.LogWarning($"Trying to move to the unreachable point {endPoint}");
				return;
			}
			// TODO: add actions to the nonempty cells
			
			// update pathfinding grid
			var gridEndCoord   = ConvertTilemapToGridCoords(endPoint);
			var gridStartCoord = ConvertTilemapToGridCoords(hero.Position);
			
			_grid.nodes[gridEndCoord.x, gridEndCoord.y].price     = 999f;
			_grid.nodes[gridStartCoord.x, gridStartCoord.y].price = CalcPathPriceForCell(hero.Position);
			// move hero
			hero.Position = endPoint;
			MapChanged?.Invoke();
		}

		Point ConvertTilemapToGridCoords(Vector3Int tilemapCoord) {
			var point = tilemapCoord - _mapSizesBounds.min;
			return new Point(point.x, point.y);
		}
		
		PathGrid CreatePathfindingGrid() {
			var weights = new float[_mapSizesBounds.size.x, _mapSizesBounds.size.y];
			for (var y = 0; y < _mapSizesBounds.size.y; y++) {
				for (var x = 0; x < _mapSizesBounds.size.x; x++) {
					var pos = _mapSizesBounds.min + new Vector3Int(x, y, 0);
					weights[x, y] = CalcPathPriceForCell(pos);
				}
			}
			return new PathGrid(weights);
		}

		float CalcPathPriceForCell(Vector3Int position) {
			var heroStates = _heroController.GetAllHeroes();
			var cities     = _mapInfo.MapCities;
			return !cities.Exists(obj => obj.Position == position) && !heroStates.Exists(obj => obj.MapPosition == position)
					   ? 1
					   : 999f;
		}
	}
}