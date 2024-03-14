using System.Collections.Generic;
using UnityEngine;
using GO_Device;
using UnityEngine.Events;
using UnityEngine.Assertions;

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
            public DeviceBase device = null ;
            // UI register a callback function, if track change precition, call this function
            public UnityEvent<float> TrackPrecChangeCallUI = new UnityEvent<float>();
            // Track register a callback function, if ui change precition, call this funciton
            public UnityEvent<float> UIPrecChangeCallTrack = new UnityEvent<float>();
        }

        public List<TrackSlideInfo> DevicesOnTrack;

        public void MovePosition(DeviceBase device, float prec) {
            prec = Mathf.Clamp01(prec);
            Vector3 newPosition = Vector3.Lerp(Head.position, Tail.position, prec);
            Vector3 newRelPosition = newPosition - Head.position;

            Vector3 deviceOrigPosition = device.transform.position;
            Vector3 deviceRelPos = deviceOrigPosition - Head.position;
            Vector3 orthognalComponent = deviceRelPos - Vector3.Dot(deviceRelPos, newRelPosition) / newRelPosition.magnitude * newRelPosition.normalized;


            device.transform.position = Head.position + newRelPosition + orthognalComponent;
        }

        public float GetPrec(TrackSlideInfo slideInfo)
        {
            Assert.IsNotNull(slideInfo);
            Assert.IsNotNull(slideInfo.device);
            Vector3 position = slideInfo.device.transform.position;
            Vector3 deviceRelPos = position - Head.position;
            Vector3 tailRelPos = Tail.position - Head.position;
            float prec = Vector3.Dot(deviceRelPos, tailRelPos)/tailRelPos.sqrMagnitude;
            return prec;
        }

        public TrackSlideInfo AddDevice()
        {
            // Instanite Tail
            DeviceBase newDevice = GameObject.Instantiate(TempSingletonManager.Instance.PolarizerPrefab, this.transform).GetComponent<DeviceBase>();
            newDevice.transform.position = Tail.position;
            newDevice.gameObject.name = "Polarizer";
            return AddDevice(newDevice);

        }

        public TrackSlideInfo AddDevice(DeviceBase basedevice) {
            // Instanite Tail
            TrackSlideInfo slideinfo = new TrackSlideInfo();
            slideinfo.device = basedevice;

            slideinfo.UIPrecChangeCallTrack.AddListener(
                (val)=>{
                    MovePosition(slideinfo.device, val);
                }
            );


            basedevice.transform.SetParent(this.transform);

            DevicesOnTrack.Add(slideinfo);
            return slideinfo;

        }

        public void RemoveDevice(TrackSlideInfo sliderInfo) {
            for (int i = 0; i < DevicesOnTrack.Count; i++)
            {
                if (DevicesOnTrack[i] == sliderInfo)
                {
                    DevicesOnTrack[i].TrackPrecChangeCallUI.RemoveAllListeners();
                    DevicesOnTrack[i].UIPrecChangeCallTrack.RemoveAllListeners();
                    DevicesOnTrack.RemoveAt(i);
                    break;
                }
            }

            Destroy(sliderInfo.device.gameObject);
        }

        void Start() {
            List<DeviceBase> devices = new List<DeviceBase>(GetComponentsInChildren<DeviceBase>());

            DevicesOnTrack = new List<TrackSlideInfo>();
            foreach (var device in devices)
            {
                AddDevice(device);
            }

        }
    }
}