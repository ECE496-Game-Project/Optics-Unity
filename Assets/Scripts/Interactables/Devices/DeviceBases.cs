using UnityEngine;
using GO_Wave;
using Interfaces;
using ParameterTransfer;

namespace GO_Device {
    public enum DEVICETYPE {
        INVALID = 0,
        POLARIZER,
        WEAVEPLATE
    }

	public class DeviceBase : MonoBehaviour, I_ParameterTransfer {
        public DEVICETYPE DeviceType = DEVICETYPE.INVALID;
        public virtual void WaveHit(in RaycastHit hit, WaveSource parentWS) { }
        public virtual void CleanDeviceHitTrace(WaveSource parentWS) { }

        public virtual void RegisterParametersCallback(ParameterInfoList ParameterInfos) {
            var RotDegTuple = (ParameterInfo<int>)ParameterInfos.SymbolQuickAccess["DeviceType"];
            RotDegTuple.Getter = () => { return (int)DeviceType; };
            RotDegTuple.Default = (int)DeviceType;
            RotDegTuple.Setter = (evt) => { DeviceType = (DEVICETYPE)evt.newValue; ParameterChangeTrigger(); };
        }
        public virtual void ParameterChangeTrigger() { }
    }
}