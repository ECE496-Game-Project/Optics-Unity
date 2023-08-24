﻿using UnityEditor;
using UnityEngine;
using GO_Wave;

namespace ns_Editor {
    [CustomEditor(typeof(RootWaveSource))]
    public class WaveSourceEditor : Editor {

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            RootWaveSource waveSource = (RootWaveSource)target;

            if (GUILayout.Button("Parameter NonDestructable Callback")) {
                waveSource.ParamNonDestructCallback();
            }
            if (GUILayout.Button("Parameter Destructable Callback")) {
                waveSource.ParamDestructCallback();
            }
        }
    }
}