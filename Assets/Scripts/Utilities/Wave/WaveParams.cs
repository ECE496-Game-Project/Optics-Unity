using UnityEngine;
using CommonUtils;

namespace WaveUtils {
	public enum WAVETYPE {
		INVALID = 0,
		PARALLEL,
		POINT
    }

	[System.Serializable]
	public class WaveParams {
		#region GLOBAL VAR
		public WAVETYPE Type = WAVETYPE.INVALID;

        public del_Vec3GetterVec3Param UHat = DefaultVec3Getter;
		public del_Vec3GetterVec3Param VHat = DefaultVec3Getter;
		public del_Vec3GetterVec3Param KHat = DefaultVec3Getter;

		public del_Vec3Getter Origin = DefaultOriginGetter;

		[Header("Magnitude Settings")]
		public float Eox;
		public float Eoy;

		[Header("Frequency Settings")]
		public float W;
		public float K;
		public float N;

		[Header("Angle Settings in degree")]
		[Range(0, 360)]
		public float Theta;
		[Range(0, 360)]
		public float Phi;
        #endregion

        #region CONSTRUCTOR
        public WaveParams(WaveParams src) {
			this.Type = src.Type;
			this.UHat = src.UHat;
			this.VHat = src.VHat;
            this.KHat = src.KHat;
			this.Origin = src.Origin;
			this.Eox = src.Eox;
			this.Eoy = src.Eoy;
			this.W = src.W;
			this.K = src.K;
			this.N = src.N;
			this.Theta = src.Theta;
			this.Phi = src.Phi;
		}
        #endregion

        #region METHOD
		public void InitLineWaveParam(del_Vec3GetterVec3Param UHat, del_Vec3GetterVec3Param VHat, del_Vec3GetterVec3Param KHat) {
			this.UHat = UHat;
			this.VHat = VHat;
			this.KHat = KHat;
		}

		public void InitPointWaveParam(del_Vec3Getter Origin) {
			this.Origin = Origin;
		}
		#endregion

		#region STATIC METHOD
		public static Vector3 DefaultVec3Getter(in Vector3 r) {
			DebugLogger.Warning("WaveParam", "del_Vec3Getter Invoke DefaultVec3Getter, return Vector3.zero");
			return Vector3.zero;
		}

		public static Vector3 DefaultOriginGetter() {
			DebugLogger.Warning("WaveParam", "del_Vec3Getter Invoke DefaultVec3Getter, return Vector3.zero");
			return Vector3.zero;
		}
        #endregion
    }
}