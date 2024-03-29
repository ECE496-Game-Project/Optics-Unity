﻿using UnityEditor;
using UnityEngine;
using GO_Device;

namespace ns_Editor {
    [CustomEditor(typeof(PolarizedDevice))]
    public class PolarizedDeviceEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            PolarizedDevice db = (PolarizedDevice)target;

            if (GUILayout.Button("ParameterChangeTrigger")) {
                db.ParameterChangeTrigger();
            }
        }
    }
}