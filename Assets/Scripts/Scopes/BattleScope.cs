using Hmm3Clone.Behaviour.BattleScene;
using Hmm3Clone.Manager.Battle;
using Hmm3Clone.Scopes.Mediators;
using VContainer;
using VContainer.Unity;

namespace Hmm3Clone.Scopes {
	public class BattleScope : LifetimeScope {
		public StartPointPlacer StartPointPlacer;
		
		protected override void Configure(IContainerBuilder builder) {
			base.Configure(builder);
			// Add test data
			var testData = new MapToBattleSceneData();
			builder.RegisterInstance(testData);
			// Add Start points 
 			builder.RegisterInstance(StartPointPlacer.Configuration);
			// Add manager
			builder.Register<BattleManager>(Lifetime.Scoped);
		}
	}
}