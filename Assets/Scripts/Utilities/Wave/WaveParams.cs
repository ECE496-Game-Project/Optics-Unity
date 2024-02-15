using System;
using UnityEngine;

namespace WaveUtils {
	public enum WAVETYPE {
		PLANE = 1,
		SPHERE = 2,
        INVALID = 0,
    }
    [Serializable]
	public class WaveSourceParam {
		#region GLOBAL VAR
		public WAVETYPE Type;

		[HideInInspector] public Vector3 Origin;
		[HideInInspector] public Vector3 UHat;
		[HideInInspector] public Vector3 VHat;
		[HideInInspector] public Vector3 KHat;

		[Header("Amplitude Settings")]
		public float Eox;
		public float Eoy;
		[Range(0, 360)]
		public float theta;

		[Header("Vacuum Wavelength (nm)")]
		public float lambda;

		[Header("Light Transimitting Material's Refractive Index")]
		[Range(1, 5)]
		public float n;

		[Header("Dispersion Distance")]
		public float RODistance;
		#endregion

		public WaveSourceParam(WaveSourceParam param) {
			Type = param.Type;
			Origin = param.Origin;
			UHat = param.UHat;
			VHat = param.VHat;
			KHat = param.KHat;
			Eox = param.Eox;
			Eoy = param.Eoy;
			theta = param.theta;
			lambda = param.lambda;
			n = param.n;
			RODistance = param.RODistance;
        }
	}
	public class WaveParam {
		public WAVETYPE Type;

		public Vector3 Origin;
        public Vector3 UHat;
		public Vector3 VHat;
		public Vector3 KHat;

		public float Eox;
		public float Eoy;
		public float theta;

		public float T;
		public float mu;
		public float w;

		public float lambda;
		public float f;
		public float k;

		public float phi;
		public float n;

		public float RODistance;

		public WaveParam() { }
		public WaveParam(WaveSourceParam wsParam) {
			Type = wsParam.Type;

			Origin = wsParam.Origin;
			UHat = wsParam.UHat;
			VHat = wsParam.VHat;
			KHat = wsParam.KHat;

			Eox = wsParam.Eox;
			Eoy = wsParam.Eoy;
			theta = wsParam.theta;

			lambda = wsParam.lambda;
			n = wsParam.n;
			RODistance = wsParam.RODistance;

			modifyLambda(); 
			phi = 0; 
		}
		public WaveParam(WAVETYPE type, Vector3 origin, Vector3 uHat, Vector3 vHat, Vector3 kHat, float eox, float eoy, float theta, float t, float mu, float w, float lambda, float f, float k, float phi, float n, float roDistance) {
			Type = type;
			Origin = origin;
			UHat = uHat;
			VHat = vHat;
			KHat = kHat;
			Eox = eox;
			Eoy = eoy;
			this.theta = theta;
			T = t;
			this.mu = mu;
			this.w = w;
			this.lambda = lambda;
			this.f = f;
			this.k = k;
			this.phi = phi;
			this.n = n;
			RODistance = roDistance;
		}

		public void modifyLambda() {
			f = 1 / lambda;
			k = 2 * Mathf.PI * f;
			w = WaveAlgorithm.C * k / n;
			T = 2 * Mathf.PI / w;
			mu = 1 / T;
		}
	}
}