using System.Collections.Generic;
using Hmm3Clone.Config;
using Hmm3Clone.Gameplay;
using Hmm3Clone.State;
using Hmm3Clone.Utils;
using UnityEngine;
using UnityEngine.Assertions;

namespace Hmm3Clone.Controller {
	public class HeroController : IController {
		public const string TestHeroName = "TestGuestHero";
		public const string TestHeroName2 = "TestGuestHero2";
		
		HeroControllerState _state;

		HeroConfig _config;
		
		public HeroController(TurnController turnController, HeroControllerState state) {
			_state  = state;
			_config = ConfigLoader.LoadConfig<HeroConfig>();

			// TODO: remove after testing. This code is only for testing 
			if (_state.GetHeroState(TestHeroName) == null) {
				CreateHero(TestHeroName, new Vector3Int(-1, -1, 0));
			}
			if (_state.GetHeroState(TestHeroName2) == null) {
				CreateHero(TestHeroName2, new Vector3Int(1, 1, 0));
			}
			
			turnController.OnTurnChanged += OnTurnChanged;
		}

		public HeroInfo GetHeroInfo(string heroName) {
			return _config.GetHeroInfo(heroName);
		}

		public Hero GetHero(string heroName) {
			var heroState = _state.GetHeroState(heroName);
			Assert.IsNotNull(heroState, $"Can't find state for hero {heroName}");
			return new Hero(heroState);
		}

		public List<HeroState> GetAllHeroes() {
			return _state.Heroes;
		}
		
		void OnTurnChanged(int turn) {
			_state.Heroes.ForEach(x => x.LeftMovementPoints = GetHeroInfo(x.HeroName).BaseMovementPoints);
		}
 		
		void CreateHero(string heroName, Vector3Int position) {
			Assert.IsNotNull(heroName);
			Assert.IsNull(_state.GetHeroState(heroName));
			var heroInfo = GetHeroInfo(heroName);
			Assert.IsNotNull(heroInfo);
			var hero = new HeroState(heroName);
			for (var stackIndex = 0; stackIndex < heroInfo.StartArmy.Length; stackIndex++) {
				hero.Stacks[stackIndex] = heroInfo.StartArmy[stackIndex].Clone();
			}
			hero.MapPosition = position;
			_state.Heroes.Add(hero);
		}

		public void RemoveHero(string name) {
			_state.Heroes.RemoveAll(x => x.HeroName == name);
		}
	}
}