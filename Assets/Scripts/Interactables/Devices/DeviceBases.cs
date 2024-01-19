using UnityEngine;
using GO_Wave;
using Interfaces;
using ParameterTransfer;
using UnityEngine.Events;

namespace GO_Device {
    

	public class DeviceBase : MonoBehaviour, I_ParameterTransfer, ISelectable {
        //public UnityEvent OnDeviceSelected;
        //public UnityEvent OnDeviceUnselected;

        public virtual void WaveHit(in RaycastHit hit, WaveSource parentWS) { }
        public virtual void CleanDeviceHitTrace(WaveSource parentWS) { }

        public virtual void RegisterParametersCallback(ParameterInfoList ParameterInfos) { }
        public virtual void ParameterChangeTrigger() { }
        
        // For Highlight Propose
        public virtual void OnMouseSelect() { 
            //OnDeviceSelected?.Invoke(); 
        }
        public virtual void OnMouseUnselect() { 
            //OnDeviceUnselected?.Invoke(); 
        }
    }
}