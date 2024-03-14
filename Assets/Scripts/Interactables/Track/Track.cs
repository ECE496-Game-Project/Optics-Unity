using System.Collections.Generic;
using UnityEngine;
using GO_Device;
using UnityEngine.Events;
namespace GO_Device {
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
        public class TrackSlideInfo{
            public DeviceBase device;
            // UI register a callback function, if track change precition, call this function
            public UnityEvent<float> TrackPrecChangeCallUI;
            // Track register a callback function, if ui change precition, call this funciton
            public UnityEvent<float> UIPrecChangeCallTrack;
        }

        public List<TrackSlideInfo> DevicesOnTrack;

        public void MovePosition(DeviceBase device, float prec) {

        }

        public void AddDevice(DeviceBase basedevice) {
            // Instanite Tail
            //TrackSlideInfo slideinfo = new TrackSlideInfo(basedevice);
            //slideinfo.UIPrecChangeCallTrack = (val){
            //    MovePosition(slideinfo.device, val);
            //};
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