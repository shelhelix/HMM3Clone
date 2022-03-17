using Hmm3Clone.Controller;
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
			
			builder.Register<ResourceController>(Lifetime.Scoped);
			builder.Register<TurnController>(Lifetime.Scoped);
			builder.Register<UnitsController>(Lifetime.Scoped);
			builder.Register<HeroController>(Lifetime.Scoped);
			builder.Register<CityController>(Lifetime.Scoped);
			builder.Register<SpriteSetupController>(Lifetime.Scoped);

			AutoSaver.Instance.State = state;

			// builder.Register<SceneTransmissionData>(Lifetime.Scoped);

			var data = new SceneTransmissionData {ActiveCityName = "TestCity"};
			builder.RegisterInstance(data);

			// builder.RegisterEntryPoint<EntryPointStarter>();
		}
	}
}