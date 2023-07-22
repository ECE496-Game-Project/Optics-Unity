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

        public del_Vec3ParamVec3Getter UHat = Del_Default.DefaultVec3ParamVec3Getter;
		public del_Vec3ParamVec3Getter VHat = Del_Default.DefaultVec3ParamVec3Getter;
		public del_Vec3ParamVec3Getter KHat = Del_Default.DefaultVec3ParamVec3Getter;

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
			this.Eox = src.Eox;
			this.Eoy = src.Eoy;
			this.W = src.W;
			this.K = src.K;
			this.N = src.N;
			this.Theta = src.Theta;
			this.Phi = src.Phi;
		}
        #endregion
	}
}