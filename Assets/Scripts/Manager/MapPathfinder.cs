using System.Collections.Generic;
using System.Linq;
using Hmm3Clone.Behaviour.Map;
using Hmm3Clone.Config;
using Hmm3Clone.Controller;
using NesScripts.Controls.PathFind;
using UnityEngine;

using PathGrid = NesScripts.Controls.PathFind.Grid;

namespace Hmm3Clone.Manager {
	public struct PathCell {
		public Vector3Int Coords;
		public float      CostFromStart;

		public PathCell(Vector3Int coords, float cost) {
			Coords        = coords;
			CostFromStart = cost;
		}
	}

	public sealed class MapPathfinder {
		public const float MaxPrice         = 1000f;

		const float DefaultTilePrice = 1f;
		
		RuntimeMapInfo _mapInfo;

		PathGrid _grid;

		HeroController _heroController;

		BoundsInt  MapBounds => _mapInfo.MapBounds;
		Vector3Int MapSize   => MapBounds.size; 

		public MapPathfinder(HeroController heroController, RuntimeMapInfo mapInfo) {
			_mapInfo        = mapInfo;
			_heroController = heroController;
			_grid           = CreatePathfindingGrid();
		}

		public List<PathCell> CreatePath(Vector3Int start, Vector3Int end) {
			if (!MapBounds.Contains(start) || !MapBounds.Contains(end)) {	
				return null;
			}
			var cellStart = ConvertTilemapToGridCoords(start);
			var cellEnd   = ConvertTilemapToGridCoords(end);
			
			var res       = new List<PathCell>();
			var totalCost = 0f;
			foreach (var cell in Pathfinding.FindPath(_grid, cellStart, cellEnd)) {
				var cellPrice = _grid.nodes[cell.x, cell.y].price;
				totalCost += Mathf.Approximately(cellPrice, MaxPrice) ? DefaultTilePrice : cellPrice;
				res.Add(new PathCell(ConvertGridToTilemapCoords(cell), totalCost));
			}

			return res;
		}

		public void OnHeroMoved(Vector3Int startPoint, Vector3Int endPoint) {
			var gridEndCoord   = ConvertTilemapToGridCoords(endPoint);
			var gridStartCoord = ConvertTilemapToGridCoords(startPoint);
			
			_grid.nodes[gridEndCoord.x, gridEndCoord.y].price     = MaxPrice;
			_grid.nodes[gridStartCoord.x, gridStartCoord.y].price = CalcPathPriceForCell(startPoint);
		}

		public float[,] GetCostMap() {
			var costMap = new float[_grid.nodes.GetLength(0), _grid.nodes.GetLength(1)];
			for (var x = 0; x < costMap.GetLength(0); x++) {
				for (var y = 0; y < costMap.GetLength(1); y++) {
					costMap[x, y] = _grid.nodes[x, y].price;
				}
			}

			return costMap;
		}
		
		PathGrid CreatePathfindingGrid() {
			var weights = new float[MapSize.x, MapSize.y];
			for (var y = 0; y < MapSize.y; y++) {
				for (var x = 0; x < MapSize.x; x++) {
					var pos = MapBounds.min + new Vector3Int(x, y, 0);
					weights[x, y] = CalcPathPriceForCell(pos);
				}
			}
			return new PathGrid(weights);
		}

		Point ConvertTilemapToGridCoords(Vector3Int tilemapCoord) {
			var point = tilemapCoord - MapBounds.min;
			return new Point(point.x, point.y);
		}

		Vector3Int ConvertGridToTilemapCoords(Point point) {
			return new Vector3Int(point.x, point.y, 0) + MapBounds.min;
		}
		
		float CalcPathPriceForCell(Vector3Int position) {
			var heroStates = _heroController.GetAllHeroes();
			var cities     = _mapInfo.GameplayMapInfo.MapCities;
			if (cities.Exists(obj => obj.Position == position) || heroStates.Exists(obj => obj.MapPosition == position)) {
				return MaxPrice;
			}
			if (_mapInfo.HasObjectOnCell(position)) {
				// 0 - is unwalkable cell
				return 0f;
			}
			return DefaultTilePrice;
		}
	}
}