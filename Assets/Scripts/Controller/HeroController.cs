using Hmm3Clone.Gameplay;
using Hmm3Clone.State;
using UnityEngine.Assertions;

namespace Hmm3Clone.Controller {
	public class HeroController : IController {
		public const string TestHeroName = "TestGuestHero";
		
		HeroControllerState _state;

		public HeroController(HeroControllerState state) {
			_state = state;
			
			// TODO: remove after testing. This code is only for testing 
			if (_state.GetHeroState(TestHeroName) == null) {
				var heroState = new HeroState(TestHeroName);
				_state.Heroes.Add(heroState);
			}
		}

		public Hero GetHero(string heroName) {
			var heroState = _state.GetHeroState(heroName);
			Assert.IsNotNull(heroState, $"Can't find state for hero {heroName}");
			return new Hero(heroState);
		}
	}
}