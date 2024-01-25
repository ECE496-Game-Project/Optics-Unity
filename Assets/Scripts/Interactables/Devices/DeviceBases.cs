using UnityEngine;
using GO_Wave;
using Interfaces;
using ParameterTransfer;
using UnityEngine.Events;

namespace GO_Device {
    

	public class DeviceBase : MonoBehaviour, I_ParameterTransfer {
        public virtual void WaveHit(in RaycastHit hit, WaveSource parentWS) { }
        public virtual void CleanDeviceHitTrace(WaveSource parentWS) { }

        public virtual void RegisterParametersCallback(ParameterInfoList ParameterInfos) { }
        public virtual void ParameterChangeTrigger() { }
    }
}