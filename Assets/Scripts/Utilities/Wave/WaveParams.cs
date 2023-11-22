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
		public float lambda;
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
		public WaveParams() { }
		public WaveParams(WaveParams src) {
			this.Type = src.Type;
			this.Eox = src.Eox;
			this.Eoy = src.Eoy;
			this.T = src.T;
			this.n = src.n;
			this.theta = src.theta;
			this.phi = src.phi;
			this.RODistance = src.RODistance;
		}
        #endregion
    }
}