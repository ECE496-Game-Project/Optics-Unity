using System;
using UnityEngine;
using UnityEngine.Events;
using CommonUtils;

namespace WaveUtils {
	public enum WAVETYPE {
		INVALID = 0,
		PARALLEL = 1,
		POINT = 2,
	}

	[System.Serializable]
	public class WaveParams {
		#region GLOBAL VAR
		public WAVETYPE Type;

        //public Vector3 _rotation = Vector3.zero;
        //public Vector3 _position = Vector3.zero;
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
		//[Range(0, 360)]
		public float Theta;
		//[Range(0, 360)]
		public float Phi;

		[Header("Dispersion Distance")]
		public float EffectDistance;
        #endregion

        #region CONSTRUCTOR
        public WaveParams() {
            
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
			this.EffectDistance = src.EffectDistance;
        }
        #endregion
    }
}