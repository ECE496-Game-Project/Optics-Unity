using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Assertions;
using GO_Wave;
using System.Collections;
using Panel;

namespace GO_Device {
    /// <summary>
    /// 
    /// </summary>
    /// 
    public class TrackSlideInfo {
        public DeviceBase device = null;
        // UI register a callback function, if track change precition, call this function
        public UnityEvent<float> TrackPrecChangeCallUI = new UnityEvent<float>();
        // Track register a callback function, if ui change precition, call this funciton
        public UnityEvent<float> UIPrecChangeCallTrack = new UnityEvent<float>();
    }

    public class Track : MonoBehaviour {

        /// <summary>
        /// Head and Tail should be preseted in Inspector
        /// </summary>
        public Transform Head;
        public Transform Tail;

        public List<TrackSlideInfo> DevicesOnTrack;
        public WaveSource WsOnTrack;
        IEnumerator WaitOneFrameThenEmit() {
            // Wait for one frame
            yield return null;
            WsOnTrack.Emit();
        }

        public TrackSlideInfo GetTrackSlideInfo(DeviceBase device)
        {
            foreach (var deviceOnTrack in DevicesOnTrack)
            {
                if (deviceOnTrack.device == device)
                {
                    return deviceOnTrack;
                }
            }
            return null;
        }

        public void MovePosition(DeviceBase device, float prec) {
            prec = Mathf.Clamp01(prec);
            Vector3 newPosition = Vector3.Lerp(Head.position, Tail.position, prec);
            Vector3 newRelPosition = newPosition - Head.position;

            Vector3 deviceOrigPosition = device.transform.position;
            Vector3 deviceRelPos = deviceOrigPosition - Head.position;
            Vector3 orthognalComponent = deviceRelPos - Vector3.Dot(deviceRelPos, newRelPosition) / newRelPosition.magnitude * newRelPosition.normalized;


            device.transform.position = Head.position + newRelPosition + orthognalComponent;

            StartCoroutine(WaitOneFrameThenEmit());
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
            DeviceBase newDevice = Instantiate(TempSingletonManager.Instance.PolarizerPrefab, this.transform).GetComponent<DeviceBase>();
            newDevice.transform.position = Tail.position;
            newDevice.gameObject.name = "PolarizerAdded"+ DevicesOnTrack.Count;

            var device = AddDevice(newDevice);
            StartCoroutine(WaitOneFrameThenEmit());

            return device;
        }

        public TrackSlideInfo AddDevice(DEVICETYPE type, float position)
        {
            // Instanite Tail
            DeviceBase newDevice = null;
            if(type == DEVICETYPE.POLARIZER)
                newDevice = Instantiate(TempSingletonManager.Instance.PolarizerPrefab, this.transform).GetComponent<DeviceBase>();
            else if(type == DEVICETYPE.WEAVEPLATE)
                newDevice = Instantiate(TempSingletonManager.Instance.WaveplatePrefab, this.transform).GetComponent<DeviceBase>();

            newDevice.transform.position = Tail.position;
            newDevice.transform.localScale = new Vector3(1, 1, 0.02f);
            MovePosition(newDevice, position);
            newDevice.gameObject.name = type.ToString() + DevicesOnTrack.Count;

            var device = AddDevice(newDevice);
            StartCoroutine(WaitOneFrameThenEmit());

            return device;
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
            if(ParamPanelManager.Instance.currentObj == sliderInfo.device.gameObject)
                ParamPanelManager.Instance.SelectParamView(null);

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

            StartCoroutine(WaitOneFrameThenEmit());
        }

        void Start() {
            List<DeviceBase> devices = new List<DeviceBase>(GetComponentsInChildren<DeviceBase>());
            if(WsOnTrack == null) WsOnTrack = GetComponentInChildren<WaveSource>();
            if (WsOnTrack == null) Debug.LogError("Track cant find Wavesource!");
            DevicesOnTrack = new List<TrackSlideInfo>();
            foreach (var device in devices)
            {
                AddDevice(device);
            }

        }
    }
}