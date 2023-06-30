using UnityEngine;
using GO_Wave;

namespace WaveUtils {
	public enum SAMPLETYPE {
        INVALID = 0,
        LINE,
        PLANE,
        CIRCLE
    }
    public class WaveSamples {
        public WaveSource ws;
        public int count;
        public Vector3[] samples;
    }
}