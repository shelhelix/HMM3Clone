using Hmm3Clone.Behaviour.Map;
using Hmm3Clone.Controller;
using Hmm3Clone.Manager;
using VContainer;
using VContainer.Unity;

namespace Hmm3Clone.Scopes {
	public class MapScope : LifetimeScope {
		public RuntimeMapInfo MapInfo;
		
		protected override void Configure(IContainerBuilder builder) {
			builder.RegisterEntryPoint<MapStarter>();
			builder.RegisterComponent(MapInfo);
			builder.Register<DeadMapObjectsController>(Lifetime.Scoped);
			builder.Register<NeutralArmyController>(Lifetime.Scoped);
			builder.Register<MapManager>(Lifetime.Scoped);
		}

	}
}