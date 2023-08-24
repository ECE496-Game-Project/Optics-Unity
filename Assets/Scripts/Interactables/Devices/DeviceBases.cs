using UnityEngine;
using GO_Wave;

namespace GO_Device {
    public enum DEVICETYPE {
        INVALID = 0,
        POLARIZER,
        WEAVEPLATE
    }

	public class DeviceBase : MonoBehaviour {
        public DEVICETYPE type = DEVICETYPE.INVALID;
        public virtual void WaveHit(in RaycastHit hit, WaveSource parentWS) { }
        public virtual void WaveClean(WaveSource parentWS) { }
    }
}