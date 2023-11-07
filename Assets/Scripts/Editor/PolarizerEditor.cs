using UnityEditor;
using UnityEngine;
using GO_Device;

namespace ns_Editor {
    [CustomEditor(typeof(Polarizer))]
    public class PolarizerEditor : Editor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            Polarizer db = (Polarizer)target;

            if (GUILayout.Button("ParameterChangeTrigger")) {
                db.ParameterChangeTrigger();
            }
        }
    }
}