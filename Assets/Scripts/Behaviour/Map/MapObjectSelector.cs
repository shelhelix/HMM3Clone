using System;
using System.Collections.Generic;
using Hmm3Clone.Behaviour.Common;
using Hmm3Clone.Controller;
using Hmm3Clone.Manager;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using VContainer;

namespace Hmm3Clone.Behaviour.Map {
	public class MapObjectSelector : BaseInjectableComponent {
		[Inject]
		RuntimeMapInfo _mapInfo;

		[Inject]
		HeroController _heroController;
		
		[Inject]
		MapManager     _mapManager;
		
		
		Dictionary<Tilemap, Action> _clickProcessors;

		Vector3 PressedScreenPoint {
			get {
				var point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				point.z = 0;
				return point;
			}
		}

		[Inject]
		void Init() {
			_clickProcessors = new Dictionary<Tilemap, Action> {
				{_mapInfo.Heroes, ProcessHeroLayerClick},
				{_mapInfo.Terrain, ProcessTerrainLayer}
			};
		}
		
		void Update() {
			if (Input.GetMouseButtonDown(0)) {
				ProcessClick();
			}
		}

		void ProcessClick() {
			var worldPoint    = PressedScreenPoint;
			var activeTilemap = GetPressedTilemap(worldPoint);
			if (!activeTilemap) {
				return;
			}
			if (_clickProcessors.TryGetValue(activeTilemap, out var layerProcessor)) {
				layerProcessor();
			}
		}

		void ProcessHeroLayerClick() {
			var cellPosition = _mapInfo.Heroes.WorldToCell(PressedScreenPoint);
			var pressedHero  = _heroController.GetAllHeroes().Find(x => x.MapPosition == cellPosition);
			Assert.IsNotNull(pressedHero);
			_mapManager.SelectHero(pressedHero.HeroName);
			// TODO: move -> attack/trade hero or set point to the her
		}

		void ProcessTerrainLayer() {
			var cellPosition = _mapInfo.Heroes.WorldToCell(PressedScreenPoint);
			var hero         = _heroController.GetHero(_mapManager.SelectedHeroName);
			if (hero.PathEndPoint == cellPosition) {
				_mapManager.MoveHero(_mapManager.SelectedHeroName, cellPosition);
			} else {
				_mapManager.SetPathEndPointForSelectedHero(cellPosition);	
			}
		}

		Tilemap GetPressedTilemap(Vector3 worldPoint) {
			var hit = Physics2D.Raycast(worldPoint, Vector2.zero);
			if (!hit.collider) {
				return null;
			}
			hit.collider.gameObject.TryGetComponent<Tilemap>(out var res);
			return res;
		}
	}
}