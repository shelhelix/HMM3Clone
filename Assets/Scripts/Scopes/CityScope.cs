using Hmm3Clone.Behaviour;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using UnityEngine.Assertions;
using VContainer;
using VContainer.Unity;

namespace Hmm3Clone.Scopes {
	public class CityScope : LifetimeScope {
		protected override void Configure(IContainerBuilder builder) {
			builder.RegisterEntryPoint<CitySceneStarter>();

			builder.Register(resolver => {
				var transmissionData = resolver.Resolve<SceneTransmissionData>();
				var cityController   = resolver.Resolve<CityController>();
				var cityName         = transmissionData.ActiveCityName;
				// var cityName = "TestCity";
				var res      = cityController.GetCityState(cityName);
				Assert.IsNotNull(res);
				return res;
			}, Lifetime.Scoped);
			
		}
	}
}