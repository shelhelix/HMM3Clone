
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.Gameplay;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Hmm3Clone.Behaviour.Map {
	public class MapStarter : GameComponent{
		[NotNull] public Tilemap  HeroLayer;
		[NotNull] public TileBase HeroTile;

		Hero _hero;
		
		void Start() {
			var heroController = GameController.Instance.GetController<HeroController>();
			_hero = heroController.GetHero(HeroController.TestHeroName);
			PlaceHeroTile(_hero.Position);
		}

		void PlaceHeroTile(Vector3Int position) {
			HeroLayer.SetTile(_hero.Position, null);
			_hero.Position = position;
			HeroLayer.SetTile(position, HeroTile);
		}

		void Update() {
			if (!Input.GetMouseButtonDown(0)) {
				return;
			}
			
			var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			worldPos.z = 0;
			var cellPos  = HeroLayer.WorldToCell(worldPos);
			PlaceHeroTile(cellPos);
		}
	}
}