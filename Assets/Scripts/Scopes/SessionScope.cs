using Hmm3Clone.Controller;
using Hmm3Clone.Scopes.Mediators;
using Hmm3Clone.Service;
using Hmm3Clone.State;
using Hmm3Clone.Utils;
using VContainer;
using VContainer.Unity;

namespace Hmm3Clone.Scopes {
	public class SessionScope : LifetimeScope {
		protected override void Configure(IContainerBuilder builder) {
			var state =  SaveUtils.LoadState() ?? new GameState();

			builder.RegisterInstance(state);
			builder.RegisterInstance(state.HeroState);
			builder.RegisterInstance(state.MapState);
			builder.RegisterInstance(state.ResourcesState);
			builder.RegisterInstance(state.TurnState);
			
			AddController<ResourceController>(builder);
			AddController<TurnController>(builder);
			AddController<UnitsController>(builder);
			AddController<HeroController>(builder);
			AddController<CityController>(builder);
			AddController<SpriteSetupController>(builder);

			AutoSaver.Instance.State = state;

			builder.Register<MapToCitySceneData>(Lifetime.Singleton);
			builder.Register<MapToBattleSceneData>(Lifetime.Singleton);
			builder.Register<BattleToMapData>(Lifetime.Singleton);
		}

		void AddController<T>(IContainerBuilder builder) {
			builder.Register<T>(Lifetime.Scoped);
		}
	}
}