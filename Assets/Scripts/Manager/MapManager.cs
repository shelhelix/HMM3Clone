using System;
using System.Collections.Generic;
using System.Linq;
using Hmm3Clone.Behaviour.Map;
using Hmm3Clone.Controller;
using Hmm3Clone.DTO;
using NesScripts.Controls.PathFind;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hmm3Clone.Manager {
	public class MapManager {
		readonly HeroController _heroController;
		readonly MapPathfinder  _pathFinder;
		readonly RuntimeMapInfo _mapInfo;
		
		public event Action MapChanged;

		BoundsInt _mapSizesBounds;
		
		public MapManager(HeroController heroController, RuntimeMapInfo mapInfo) {
			_heroController = heroController;
			_mapInfo        = mapInfo;
			_pathFinder     = new MapPathfinder(heroController, mapInfo);
		}

		public void MoveHero(string heroName, Vector3Int endPoint) {
			var hero = _heroController.GetHero(heroName);
			var path = _pathFinder.CreatePath(hero.Position, endPoint);
			if (path == null || path.Count == 0) {
				Debug.LogWarning($"Trying to move to the unreachable point {endPoint}");
				return;
			}
			InteractWithNonEmptyCells(endPoint);
			var oldPosition = hero.Position;
			hero.Position = endPoint;
			_pathFinder.OnHeroMoved(oldPosition, endPoint);
			MapChanged?.Invoke();
		}

		void InteractWithNonEmptyCells(Vector3Int endPosition) {
			if (_mapInfo.IsCityCell(endPosition)) {
				ShowCity(_mapInfo.GetCityName(endPosition));	
			}
		}
		
		void ShowCity(string cityName) {
			ActiveData.Instance.SetData(new CityViewInitData(cityName));
			SceneManager.LoadScene("CityView");
		}


		public float[,] GetCostArray() {
			return _pathFinder.GetCostMap();
		}
	}
}