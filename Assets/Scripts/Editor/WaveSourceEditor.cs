using UnityEditor;
using UnityEngine;
using GO_Wave;

namespace ns_Editor {
    [CustomEditor(typeof(RootWaveSource))]
    public class WaveSourceEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            RootWaveSource waveSource = (RootWaveSource)target;

            if (GUILayout.Button("Clean Callback")) {
                waveSource.CleanCallback();
            }
            if (GUILayout.Button("Destructable Callback")) {
                waveSource.DestructCallback();
            }
        }
    }
}