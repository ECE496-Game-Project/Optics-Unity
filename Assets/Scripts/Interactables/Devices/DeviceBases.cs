using UnityEngine;
using GO_Wave;
using WaveUtils;

namespace GO_Device {
    public enum DEVICETYPE {
        INVALID = 0,
        POLARIZER,
        WEAVEPLATE
    }

	public class DeviceBase : MonoBehaviour {
        public DEVICETYPE type = DEVICETYPE.INVALID;
        public virtual void WaveHit(in RaycastHit hit, WaveSource parentWS) { }
        public virtual void WaveCleanup(WaveSource parentWS) { }
    }

    public class PolarizeDevice : DeviceBase {
        public virtual ComplexMatrix2X2 JohnsMatrix { get; }
    }
}