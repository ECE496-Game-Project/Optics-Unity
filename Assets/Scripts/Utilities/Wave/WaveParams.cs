using System;
using UnityEngine;
using UnityEngine.Events;
using CommonUtils;

namespace WaveUtils {
	public enum WAVETYPE {
		PLANE = 1,
		SPHERE = 2,
        INVALID = 0,
    }

	[System.Serializable]
	public class WaveParams {
		#region GLOBAL VAR
		public WAVETYPE Type;

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

		[Header("Dispersion Distance")]
		public float RODistance;
        #endregion

        #region CONSTRUCTOR
        public WaveParams(WAVETYPE type, float eox, float eoy, float w, float k, float n, float theta, float phi) {
			this.Type = type;
			this.Eox = eox;
			this.Eoy = eoy;
			this.W = w;
			this.K = k;
			this.N = n;
			this.Theta = theta;
			this.Phi = phi;
		}
		public WaveParams(WaveParams src) {
			this.Type = src.Type;
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