using Hmm3Clone.Behaviour.BattleScene;
using Hmm3Clone.Gameplay;
using Hmm3Clone.Manager.Battle;
using Hmm3Clone.Scopes.Mediators;
using Hmm3Clone.State;
using VContainer;
using VContainer.Unity;

namespace Hmm3Clone.Scopes {
	public class BattleScope : LifetimeScope {
		public StartPointPlacer StartPointPlacer;
		
		protected override void Configure(IContainerBuilder builder) {
			base.Configure(builder);
			// Add test data
			var testData = new MapToBattleSceneData {
				LeftSide = new BattleSideInfo("hero", new Army(new[] {new UnitStack(UnitType.Gnoll, 10), new UnitStack(UnitType.Gnoll, 20)}), SideType.Hero),
				RightSide = new BattleSideInfo(string.Empty, new Army(new[] {new UnitStack(UnitType.AdvancedGnoll, 5), new UnitStack(UnitType.AdvancedGnoll, 1), new UnitStack(UnitType.AdvancedGnoll, 2)}), SideType.Neutral)
			};
			builder.RegisterInstance(testData);
			// Add Start points 
 			builder.RegisterInstance(StartPointPlacer.Configuration);
			// Add manager
			builder.Register<BattleManager>(Lifetime.Scoped);
		}
	}
}