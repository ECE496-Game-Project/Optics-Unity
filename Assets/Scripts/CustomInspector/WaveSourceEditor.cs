using UnityEditor;
using UnityEngine;
using GO_Wave;
namespace CustomInspector {
    [CustomEditor(typeof(WaveSource))]
    public class WaveSource : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();


        }
    }
}