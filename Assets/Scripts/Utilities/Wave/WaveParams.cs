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
		public Param<WAVETYPE> Type;

        //public Vector3 _rotation = Vector3.zero;
        //public Vector3 _position = Vector3.zero;
        public del_Vec3ParamVec3Getter UHat = Del_Default.DefaultVec3ParamVec3Getter;
		public del_Vec3ParamVec3Getter VHat = Del_Default.DefaultVec3ParamVec3Getter;
		public del_Vec3ParamVec3Getter KHat = Del_Default.DefaultVec3ParamVec3Getter;

		[Header("Magnitude Settings")]
		public Param<float> Eox;
		public Param<float> Eoy;

		[Header("Frequency Settings")]
		public Param<float> W;
		public Param<float> K;
		public Param<float> N;

		[Header("Angle Settings in degree")]
		//[Range(0, 360)]
		public Param<float> Theta;
		//[Range(0, 360)]
		public Param<float> Phi;

		[Header("Dispersion Distance")]
		public Param<float> EffectDistance;
        #endregion

        #region CONSTRUCTOR
        public WaveParams() {
            Type = new Param<WAVETYPE>();
            Eox = new Param<float>();
            Eoy = new Param<float>();
            W = new Param<float>();
            K = new Param<float>();
            N = new Param<float>();
			Theta = new Param<float>();
            Phi = new Param<float>();
			EffectDistance = new Param<float>();
		}
		public WaveParams(WaveParams src) {
            Type = new Param<WAVETYPE>();
            Eox = new Param<float>();
            Eoy = new Param<float>();
            W = new Param<float>();
            K = new Param<float>();
            N = new Param<float>();
			Theta = new Param<float>();
			Phi = new Param<float>();
			EffectDistance = new Param<float>();

			this.Type.Value = src.Type.Value;
			this.Eox.Value = src.Eox.Value;
			this.Eoy.Value = src.Eoy.Value;
			this.W.Value = src.W.Value;
			this.K.Value = src.K.Value;
			this.N.Value = src.N.Value;
			this.Theta.Value = src.Theta.Value;
			this.Phi.Value = src.Phi.Value;
			this.EffectDistance.Value = src.EffectDistance.Value;
        }
        #endregion
    }
}