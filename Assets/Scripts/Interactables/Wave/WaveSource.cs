using UnityEngine;
using WaveUtils;
using Profiles;

namespace GO_Wave {
    public class WaveSource : MonoBehaviour {

        [SerializeField] private SO_WaveParams _profile;

        public WaveParams ActiveWaveParams;
        public float EffectDistance = 100.0f;

        #region DELEGATE FUNCTION (USED FOR WAVEPARAM)
        public Vector3 GetUHatParallel(in Vector3 r) { return this.transform.right; }
        public Vector3 GetVHatParallel(in Vector3 r) { return this.transform.up; }
        public Vector3 GetKHatParallel(in Vector3 r) { return this.transform.forward; }

        // [TODO][PointWave]: PointWave UVK direction Function
        public Vector3 GetUHatPoint(in Vector3 r) { return Vector3.zero; }
        public Vector3 GetVHatPoint(in Vector3 r) { return Vector3.zero; }
        public Vector3 GetKHatPoint(in Vector3 r) { return Vector3.zero; }
        #endregion

        void Start() {
            /*init ActiveWaveParams*/
            ActiveWaveParams = new WaveParams(_profile.Parameters);
            switch (ActiveWaveParams.Type) {
                case WAVETYPE.PARALLEL: 
                    ActiveWaveParams.UHat = GetUHatParallel;
                    ActiveWaveParams.VHat = GetVHatParallel;
                    ActiveWaveParams.KHat = GetKHatParallel;
                    break;
                case WAVETYPE.POINT:
                    ActiveWaveParams.UHat = GetUHatPoint;
                    ActiveWaveParams.VHat = GetVHatPoint;
                    ActiveWaveParams.KHat = GetKHatPoint;
                    break;
                case WAVETYPE.INVALID:
                default:
                    break;
            }
        }
    }
}