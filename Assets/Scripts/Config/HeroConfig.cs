using System.Collections.Generic;
using UnityEngine;

namespace Hmm3Clone.Config {
	[CreateAssetMenu]
	public class HeroConfig : ScriptableObject {
		public List<HeroInfo> Heroes;

		public HeroInfo GetHeroInfo(string heroName) {
			return Heroes.Find(x => x.HeroName == heroName);
		}
	}
}