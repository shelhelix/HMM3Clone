using UnityEditor;

namespace Hmm3Clone.Behaviour.BattleScene.Editor {
	[CustomEditor(typeof(StartPointPlacer))]
	public class CustomDrawer : UnityEditor.Editor {
		
		public override void OnInspectorGUI() {
			EditorGUI.BeginChangeCheck();
			DrawDefaultInspector();
			if (EditorGUI.EndChangeCheck()) {
				(target as StartPointPlacer).OnValueChanged();		
			}
		}
	}
}