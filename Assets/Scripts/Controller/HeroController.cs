using System.Collections.Generic;
using Hmm3Clone.Config;
using Hmm3Clone.Gameplay;
using Hmm3Clone.State;
using Hmm3Clone.Utils;
using UnityEngine.Assertions;

namespace Hmm3Clone.Controller {
	public class HeroController : IController {
		public const string TestHeroName = "TestGuestHero";
		
		HeroControllerState _state;

		HeroConfig _config;
		
		public HeroController(TurnController turnController, HeroControllerState state) {
			_state  = state;
			_config = ConfigLoader.LoadConfig<HeroConfig>();

			// TODO: remove after testing. This code is only for testing 
			if (_state.GetHeroState(TestHeroName) == null) {
				CreateHero(TestHeroName);
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
 		
		void CreateHero(string heroName) {
			Assert.IsNotNull(heroName);
			Assert.IsNull(_state.GetHeroState(heroName));
			var heroInfo = GetHeroInfo(heroName);
			Assert.IsNotNull(heroInfo);
			var hero = new HeroState(heroName);
			for (var stackIndex = 0; stackIndex < heroInfo.StartArmy.Length; stackIndex++) {
				hero.Stacks[stackIndex] = heroInfo.StartArmy[stackIndex].Clone();
			}
			_state.Heroes.Add(hero);
		}
	}
}