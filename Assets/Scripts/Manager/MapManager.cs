using System;
using System.Collections.Generic;
using System.Linq;
using Hmm3Clone.Behaviour.Map;
using Hmm3Clone.Controller;
using Hmm3Clone.DTO;
using Hmm3Clone.Gameplay;
using Hmm3Clone.State;
using Hmm3Clone.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace Hmm3Clone.Manager {
	public class MapManager {
		readonly HeroController _heroController;
		readonly MapPathfinder  _pathFinder;
		readonly RuntimeMapInfo _mapInfo;
		
		public event Action     MapChanged;

		public event Action<string> OnHeroDataChanged;
		
		public ReactValue<string> SelectedHeroName = new ReactValue<string>();

		public bool HasSelectedHero => !string.IsNullOrEmpty(SelectedHeroName.Value);
		
		public MapManager(HeroController heroController, RuntimeMapInfo mapInfo) {
			_heroController = heroController;
			_mapInfo        = mapInfo;
			_pathFinder     = new MapPathfinder(heroController, mapInfo);
		}

		public void SelectHero(string heroName) {
			SelectedHeroName.Value = heroName;
			OnHeroDataChanged?.Invoke(SelectedHeroName);
		}

		public List<PathCell> CreatePath(string heroName, Vector3Int endPoint) {
			var hero = _heroController.GetHero(heroName);
			return _pathFinder.CreatePath(hero.Position, endPoint);
		}

		public void SetPathEndPointForSelectedHero(Vector3Int position) {
			Assert.IsTrue(HasSelectedHero);
			var hero = _heroController.GetHero(SelectedHeroName);
			hero.PathEndPoint = position;
			OnHeroDataChanged?.Invoke(SelectedHeroName);
		}

		public void MoveHero(string heroName, Vector3Int endPoint) {
			var hero = _heroController.GetHero(heroName);
			var path = _pathFinder.CreatePath(hero.Position, endPoint);
			if (path == null ) {
				if (hero.PathEndPoint == endPoint) {
					hero.PathEndPoint = HeroState.InvalidPoint;
				}
				Debug.LogWarning($"Trying to move to the unreachable point {endPoint}");
				return;
			}
			path.RemoveAll(x => x.CostFromStart > hero.MovementPoints);
			if (path.Count == 0) {
				return;
			}
			var realEndPoint = path.Last();
			InteractWithNonEmptyCells(realEndPoint.Coords);
			var oldPosition = hero.Position;
			hero.Position       =  realEndPoint.Coords;
			hero.MovementPoints -= Mathf.FloorToInt(realEndPoint.CostFromStart);
			_pathFinder.OnHeroMoved(oldPosition, realEndPoint.Coords);
			MapChanged?.Invoke();
			OnHeroDataChanged?.Invoke(SelectedHeroName);
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