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

		[Header("Amplitude Settings")]
		public float Eox;
		public float Eoy;
		[Range(0, 360)]
		public float theta;

		[Header("Temporal Freq Settings")]
		public float T;
		public float mu;
		public float w;

		[Header("Spatial Freq Settings")]
		public float Lambda;
		public float f;
		public float k;

		[Range(0, 360)]
		public float phi;
		[Range(1, 5)]
		public float n;

		[Header("Dispersion Distance")]
		public float RODistance;
        #endregion

        #region CONSTRUCTOR
        public WaveParams(WAVETYPE type, float eox, float eoy, float w, float k, float n, float theta, float phi) {
			this.Type = type;
			this.Eox = eox;
			this.Eoy = eoy;
			this.w = w;
			this.k = k;
			this.n = n;
			this.theta = theta;
			this.phi = phi;
		}
		public WaveParams(WaveParams src) {
			this.Type = src.Type;
			this.Eox = src.Eox;
			this.Eoy = src.Eoy;
			this.w = src.w;
			this.k = src.k;
			this.n = src.n;
			this.theta = src.theta;
			this.phi = src.phi;
        }
        #endregion
    }
}