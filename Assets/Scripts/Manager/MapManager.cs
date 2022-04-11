using System;
using System.Collections.Generic;
using System.Linq;
using Hmm3Clone.Behaviour.Map;
using Hmm3Clone.Controller;
using Hmm3Clone.Gameplay;
using Hmm3Clone.Scopes.Mediators;
using Hmm3Clone.State;
using Hmm3Clone.Utils;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using VContainer;

namespace Hmm3Clone.Manager {
	public class MapManager {
		[Inject] readonly HeroController        _heroController;
		[Inject] readonly CityController        _cityController;
		[Inject] readonly NeutralArmyController _neutralArmyController;

		[Inject] readonly RuntimeMapInfo        _mapInfo;
		
		[Inject] readonly MapToCitySceneData   _sceneData;
		[Inject] readonly MapToBattleSceneData _mapToBattleSceneData;
		
		[Inject] BattleToMapData _battleToMapData;
		
		MapPathfinder  _pathFinder;
		
		public event Action     MapChanged;

		public event Action<string> OnHeroDataChanged;
		
		
		public ReactValue<string> SelectedHeroName = new ReactValue<string>();

		public bool HasSelectedHero => !string.IsNullOrEmpty(SelectedHeroName.Value);

		public void Init() {
			TryProcessObjectsAfterBattle();
			_pathFinder = new MapPathfinder(_heroController, _neutralArmyController, _mapInfo);
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
			InteractWithNonEmptyFirstCell(hero.Position, heroName);
			InteractWithNonEmptyLastCell(hero, realEndPoint.Coords);
			var oldPosition = hero.Position;
			hero.Position       =  realEndPoint.Coords;
			hero.MovementPoints -= Mathf.FloorToInt(realEndPoint.CostFromStart);
			_pathFinder.OnHeroMoved(oldPosition, realEndPoint.Coords);
			MapChanged?.Invoke();
			OnHeroDataChanged?.Invoke(heroName);
		}
		
		
		public void ShowCity(string cityName) {
			_sceneData.ActiveCityName = cityName;
			SceneManager.LoadScene("CityView");
		}

		void TryProcessObjectsAfterBattle() {
			var objectToRemove = _battleToMapData.Loser;
			if (!objectToRemove.IsValid) {
				return;
			}
			switch (objectToRemove.Type) {
				case SideType.City: {
					_cityController.RemoveGarrison(objectToRemove.Name);
					break;
				}
				case SideType.Hero: {
					_heroController.RemoveHero(objectToRemove.Name);
					break;
				}
				case SideType.Neutral: {
					_neutralArmyController.RemoveNeutralArmy(VectorUtils.StringToVector3(objectToRemove.Name));
					break;
				}
			}
			MapChanged?.Invoke();
			_battleToMapData.Loser = BattleSideInfo.InvalidSide;
		}

		void TransferHeroToCity(string cityName) {
			_cityController.SetGuestHero(cityName, SelectedHeroName);
			ShowCity(cityName);
		}

		void InteractWithNonEmptyLastCell(Hero hero, Vector3Int endPosition) {
			if (_mapInfo.IsCityCell(endPosition)) {
				TransferHeroToCity(_mapInfo.GetCityName(endPosition));	
			}
			if ( _neutralArmyController.IsNeutralArmyCell(endPosition) ) {
				var neutralArmy = _neutralArmyController.GetNeutralArmyOnCell(endPosition);
				Assert.IsNotNull(neutralArmy);
				var heroSide    = new BattleSideInfo(hero.Name, hero.Army, SideType.Hero);
				var neutralSide = new BattleSideInfo(neutralArmy.Position.ToString(), new Army(neutralArmy.UnitsStacks), SideType.Neutral);
				TransferToBattleScene(heroSide, neutralSide);
			}
		}

		void TransferToBattleScene(BattleSideInfo leftSide, BattleSideInfo rightSide) {
			_mapToBattleSceneData.LeftSide  = leftSide;
			_mapToBattleSceneData.RightSide = rightSide;
			SceneManager.LoadScene("BattleScene");
		}
		
		void InteractWithNonEmptyFirstCell(Vector3Int startPosition, string heroName) {
			if (_mapInfo.IsCityCell(startPosition)) {
				_cityController.RemoveGuestHero(heroName);
			}
		}

		public float[,] GetCostArray() {
			return _pathFinder.GetCostMap();
		}
	}
}