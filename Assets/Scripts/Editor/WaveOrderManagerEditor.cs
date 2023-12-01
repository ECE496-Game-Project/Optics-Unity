using UnityEditor;
using UnityEngine;
using Constraint;
using GO_Device;

namespace ns_Editor
{
    [CustomEditor(typeof(WaveOrderManager))]
    public class WaveOrderManagerEditor : Editor
    {
        private static int firstIdx, secondIdx;

        private static int removeIdx;

        public static DEVICETYPE deviceType;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            WaveOrderManager waveSource = (WaveOrderManager)target;

            // Add a custom input field
            firstIdx = EditorGUILayout.IntField("FirstIdx", firstIdx);
            secondIdx = EditorGUILayout.IntField("SecondIdx", secondIdx);
            
            if (GUILayout.Button("Swap"))
            {
                waveSource.SwapDeviceOrder(firstIdx, secondIdx);
            }

            removeIdx = EditorGUILayout.IntField("removeIdx", removeIdx);
            if (GUILayout.Button("RemoveDevice"))
            {
                waveSource.RemoveDevice(removeIdx);
            }

            deviceType = (DEVICETYPE) EditorGUILayout.EnumPopup("DeviceType", deviceType);
            if (GUILayout.Button("AddDevice"))
            {
                waveSource.AddDevice(deviceType);
            }
        }
    }
}