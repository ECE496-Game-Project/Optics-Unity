using System.Collections.Generic;
using UnityEngine;
using GO_Device;
using UnityEngine.Events;
namespace Track {
    public class Track : MonoBehaviour {

        /// <summary>
        /// Head and Tail should be preseted in Inspector
        /// </summary>
        public Transform Head;
        public Transform Tail;

        /// <summary>
        /// 
        /// </summary>
        /// 
        public class DeviceInfo{
            public DeviceBase device;
            public float positionPerc;
            public UnityEvent UIValueChangeCallback;
        }

        private List<DeviceInfo> m_devicesOnTrack;


        public void MovePosition(DeviceInfo device) {

        }

        public void AddDevice() {
            // Instanite Tail
        }

        public void RemoveDevice(DeviceBase device) {

        }

        void Start() {
            //m_devicesOnTrack = new List<DeviceBase>(GetComponentsInChildren<DeviceBase>());
        }

        void Update() {

        }
    }
}