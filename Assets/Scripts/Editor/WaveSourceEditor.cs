//using UnityEditor;
//using UnityEngine;
//using GO_Wave;

//namespace ns_Editor {
//    [CustomEditor(typeof(WaveSource))]
//    public class WaveSourceEditor : Editor {
//        public override void OnInspectorGUI() {
//            base.OnInspectorGUI();
//            WaveSource waveSource = (WaveSource)target;

//            if (GUILayout.Button("Clean Callback")) {
//                waveSource.WaveClean();
//            }
//            if (GUILayout.Button("Destructable Callback")) {
//                waveSource.ParameterChangeTrigger();
//            }
//        }
//    }
//}