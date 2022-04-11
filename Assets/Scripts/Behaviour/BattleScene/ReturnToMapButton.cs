using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.Scopes.Mediators;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace Hmm3Clone.Behaviour.BattleScene {
	[RequireComponent(typeof(Button))]
	public class ReturnToMapButton : GameComponent {
		[NotNull] public Button Button;

		[Inject] BattleToMapData      _battleToMapData;
		[Inject] MapToBattleSceneData _mapToBattleSceneData;
		
		void Reset() {
			Button = GetComponent<Button>();
		}
		
		void Start() {
			Button.onClick.AddListener(ReturnToMap);
		}

		void ReturnToMap() {
			_battleToMapData.Loser = _mapToBattleSceneData.RightSide;
			SceneManager.LoadScene("Map");
		}
	}
}