using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Config.Map;
using Hmm3Clone.Controller;
using Hmm3Clone.Manager;
using Hmm3Clone.State;
using UnityEngine.Tilemaps;

namespace Hmm3Clone.Behaviour.Map {
	public class MapView : GameComponent {
		Tilemap _objects;
		Tilemap _heroes;

		[NotNull] public TileBase HeroTile;
		[NotNull] public TileBase CityTile;

		MapManager     _mapManager;
		HeroController _heroController;

		public void Init(HeroController heroController, MapManager mapManager, Tilemap objects, Tilemap heroes, MapInfo mapInfo) {
			_objects        = objects;
			_heroes         = heroes;
			_mapManager     = mapManager;
			_heroController = heroController;
			
			_mapManager.MapChanged += OnMapChanged;
			PlaceStaticObjects(mapInfo);
			OnMapChanged();
		}

		public void Deinit() {
			_mapManager.MapChanged -= OnMapChanged;
		}

		void PlaceStaticObjects(MapInfo mapInfo) {
			mapInfo.MapCities.ForEach(PlaceCity);
		}

		void PlaceCity(MapCityConstructionInfo cityInfo) {
			_objects.SetTile(cityInfo.Position, CityTile);
		}
		
		void OnMapChanged() {
			_heroes.ClearAllTiles();
			_heroController.GetAllHeroes().ForEach(PlaceHero);
		}

		void PlaceHero(HeroState heroState) {
			_heroes.SetTile(heroState.MapPosition, HeroTile);
		}
	}
}