using System.Collections.Generic;
using System.Linq;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Config.Map;
using Hmm3Clone.Controller;
using Hmm3Clone.Gameplay;
using Hmm3Clone.Manager;
using Hmm3Clone.State;
using UnityEngine;
using UnityEngine.Tilemaps;
using VContainer;

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

		[Inject] MapManager _mapManager;
		
		[Inject] HeroController _heroController;
		[Inject] TurnController _turnController;
		[Inject] RuntimeMapInfo _mapInfo;

		BoundsInt _mapSize;

		[Inject]
		void Init() {
			
			_objects = _mapInfo.Objects;
			_heroes  = _mapInfo.Heroes;
			_mapSize = _mapInfo.MapBounds;

			_mapManager.MapChanged        += OnMapChanged;
			_mapManager.OnHeroDataChanged += OnHeroDataChanged;
			_turnController.OnTurnChanged += OnTurnChanged;

			PlaceStaticObjects(_mapInfo.GameplayMapInfo);
			OnMapChanged();
		}

		void OnTurnChanged(int turn) {
			var heroName = _mapManager.SelectedHeroName;
			OnHeroDataChanged(heroName);
		}

		protected void OnDestroy() {
			_mapManager.MapChanged        -= OnMapChanged;
			_mapManager.OnHeroDataChanged -= OnHeroDataChanged;
			_turnController.OnTurnChanged -= OnTurnChanged;
		}

		void OnHeroDataChanged(string heroName) {
			var hero = _heroController.GetHero(heroName);
			DrawPath(heroName, _mapManager.CreatePath(heroName, hero.PathEndPoint));
		}

		void DrawPath(string heroName, List<PathCell> path) {
			if (path == null) {
				return;
			}
			var hero = _heroController.GetHero(heroName);
			SelectedPathLayer.ClearAllTiles();
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