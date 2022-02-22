using GameComponentAttributes.Attributes;
using Hmm3Clone.Behaviour.Map;
using Hmm3Clone.Controller;
using Hmm3Clone.Manager;
using Hmm3Clone.State;
using Hmm3Clone.Utils;
using VContainer;
using VContainer.Unity;

namespace Hmm3Clone.Scopes {
	public class MapScope : LifetimeScope {
		public static MapScope Instance;
		
		public RuntimeMapInfo MapInfo;
		
		protected override void Configure(IContainerBuilder builder) {
			MapInfo.PrepareMap();
			
			var state = TryProcessState();
			GameController.Instance.InitControllers(state);
			
			builder.RegisterEntryPoint<MapStarter>();
			builder.RegisterComponent(MapInfo);

			builder.RegisterInstance(GameController.Instance.GetController<HeroController>());
			
			builder.Register<MapManager>(Lifetime.Scoped);

			Instance = this;
		}

		GameState TryProcessState() {
			var state = SaveUtils.LoadState();
			if (state.FirstInitializationCompleted) {
				return state;
			}
			MapInfo.GameplayMapInfo.MapCities.ForEach(x => state.MapState.CityStates.Add(new CityState(x.CityName)));
			return state;
		}
	}
}