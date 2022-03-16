using Hmm3Clone.Behaviour.Map;
using Hmm3Clone.Manager;
using Hmm3Clone.State;
using VContainer;
using VContainer.Unity;

namespace Hmm3Clone.Scopes {
	public class MapScope : LifetimeScope {
		public static MapScope Instance;
		
		public RuntimeMapInfo MapInfo;
		
		protected override void Configure(IContainerBuilder builder) {

			builder.RegisterEntryPoint<MapStarter>();
			builder.RegisterComponent(MapInfo);
			
			builder.Register<MapManager>(Lifetime.Scoped);

			Instance = this;
		}

	}
}