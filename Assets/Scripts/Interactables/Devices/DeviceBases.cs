using UnityEngine;
using GO_Wave;
using Interfaces;
using ParameterTransfer;
using UnityEngine.Events;

namespace GO_Device {
    

	public class DeviceBase : MonoBehaviour, I_ParameterPanel {
        public virtual void WaveHit(in RaycastHit hit, Wave parentWS) { }
        public virtual void CleanDeviceHitTrace(Wave parentWS) { }

        public virtual string ParamTransferName { get; }
        public virtual void RegisterParametersCallback(ParameterInfoList ParameterInfos) { }
        public virtual void ParameterChangeTrigger() { }
    }
}