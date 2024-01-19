using UnityEngine;
using GO_Wave;
using Interfaces;
using ParameterTransfer;
using UnityEngine.Events;

namespace GO_Device {
    public enum DEVICETYPE {
        INVALID = 0,
        POLARIZER,
        WEAVEPLATE,
        QUATERWAVEPLATE,
        HALFWAVEPLATE,
    }

	public class DeviceBase : MonoBehaviour, I_ParameterTransfer, ISelectable {
        //public UnityEvent OnDeviceSelected;
        //public UnityEvent OnDeviceUnselected;

        public DEVICETYPE DeviceType = DEVICETYPE.INVALID;
        public virtual void WaveHit(in RaycastHit hit, WaveSource parentWS) { }
        public virtual void CleanDeviceHitTrace(WaveSource parentWS) { }

        public virtual void RegisterParametersCallback(ParameterInfoList ParameterInfos) {
            //var DeviceTypeTuple = (ParameterInfo<DEVICETYPE>)ParameterInfos.SymbolQuickAccess["DeviceType"];
            //DeviceTypeTuple.Getter = () => { return DeviceType; };
            //DeviceTypeTuple.Default = DeviceType;
            //DeviceTypeTuple.Setter = (evt) => { DeviceType = evt.newValue; ParameterChangeTrigger(); };
        }
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