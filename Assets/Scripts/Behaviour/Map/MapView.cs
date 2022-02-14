using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Config.Map;
using Hmm3Clone.Controller;
using Hmm3Clone.Gameplay;
using Hmm3Clone.Manager;
using Hmm3Clone.State;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace Hmm3Clone.Behaviour.Map {
	public class MapView : GameComponent {
		// runtime used only tilemap
		[NotNull] public Tilemap SelectedPathLayer;
		[NotNull] public Tilemap CostMap;
		
		Tilemap _objects;
		Tilemap _heroes;

		
		
		[NotNull] public TileBase HeroTile;
		[NotNull] public PathTile PathTile;
		[NotNull] public TileBase CityTile;
		
		[NotNull] public TileBase PlainTile;

		MapManager     _mapManager;
		HeroController _heroController;

		BoundsInt _mapSize;
		
		public void Init(HeroController heroController, MapManager mapManager, RuntimeMapInfo mapInfo) {
			_objects        = mapInfo.Objects;
			_heroes         = mapInfo.Heroes;
			_mapManager     = mapManager;
			_heroController = heroController;
			_mapSize        = mapInfo.MapBounds;
			
			_mapManager.MapChanged += OnMapChanged;
			PlaceStaticObjects(mapInfo.GameplayMapInfo);
			OnMapChanged();
		}

		public void Deinit() {
			_mapManager.MapChanged -= OnMapChanged;
		}

		public void DrawPath(string heroName, List<PathCell> path) {
			var hero = _heroController.GetHero(heroName);
			SelectedPathLayer.ClearAllTiles();
			if (path == null || path.Count == 0) {
				return;
			}
			PathTile.FullPath = path.Select(x => x.Coords).ToList();
			path.ForEach(x => DrawPathTile(hero, x));
			
			DrawCostView();
		}

		void DrawPathTile(Hero hero, PathCell cell) {
			SelectedPathLayer.SetTile(cell.Coords, PathTile);
			var canTravelInThisTurnColor = (hero.MovementPoints >= cell.CostFromStart) ? Color.white : new Color(1f, 0.3f, 0.3f, 1f);
			SelectedPathLayer.SetColor(cell.Coords, canTravelInThisTurnColor);
		}

		void DrawCostView() {
			CostMap.ClearAllTiles();
			var startIndex = _mapSize.min;
			var costArray  = _mapManager.GetCostArray();
			for (var x = 0; x < costArray.GetLength(0); x++) {
				for (var y = 0; y < costArray.GetLength(1); y++) {
					var tileCoords = startIndex + new Vector3Int(x, y, 0);
					var color = Mathf.Approximately(costArray[x, y], 0) 
									? Color.red 
									: Mathf.Approximately(costArray[x, y], MapPathfinder.MaxPrice) 
										? Color.yellow 
										: Color.green;
					CostMap.SetTile(tileCoords, PlainTile);
					CostMap.SetTileFlags(tileCoords, TileFlags.None);
					CostMap.SetColor(tileCoords, color);
				}
			}
		}

		void RefreshHeroesTilemap() {
			_heroes.ClearAllTiles();
			_heroController.GetAllHeroes().ForEach(PlaceHero);
		}

		void PlaceStaticObjects(MapInfo mapInfo) {
			mapInfo.MapCities.ForEach(PlaceCity);
		}

		void PlaceCity(MapCityConstructionInfo cityInfo) {
			_objects.SetTile(cityInfo.Position, CityTile);
		}
		
		void OnMapChanged() {
			RefreshHeroesTilemap();
		}

		void PlaceHero(HeroState heroState) {
			_heroes.SetTile(heroState.MapPosition, HeroTile);
		}
	}
}