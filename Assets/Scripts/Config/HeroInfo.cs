using Hmm3Clone.State;
using UnityEngine;

namespace Hmm3Clone.Config {
	[CreateAssetMenu]
	public class HeroInfo : ScriptableObject {
		public string      HeroName;
		public Sprite      HeroAvatar;
		public UnitStack[] StartArmy;
	}
}