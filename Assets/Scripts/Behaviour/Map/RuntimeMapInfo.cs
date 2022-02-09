using System;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Config.Map;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Hmm3Clone.Behaviour.Map {
	[Serializable]
	public class RuntimeMapInfo {
		[NotNull] public Tilemap Terrain;
		[NotNull] public Tilemap Objects;
		// use only for view
		[NotNull] public Tilemap Heroes;
		
		[NotNull] public MapInfo GameplayMapInfo;

		public BoundsInt MapBounds => Terrain.cellBounds;
		
		public void PrepareMap() {
			Terrain.CompressBounds();
			Objects.CompressBounds();
			Heroes.CompressBounds();
		}

		public bool IsCityCell(Vector3Int position) {
			return GameplayMapInfo.MapCities.Exists(x => x.Position == position);
		}

		public bool HasObjectOnCell(Vector3Int position) {
			return Objects.HasTile(position);
		}

		public string GetCityName(Vector3Int position) {
			return !IsCityCell(position) ? string.Empty : GameplayMapInfo.MapCities.Find(x => x.Position == position).CityName;
		}
	}
}