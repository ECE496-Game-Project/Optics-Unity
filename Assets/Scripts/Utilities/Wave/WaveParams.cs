using System;
using UnityEngine;
using CommonUtils;

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
		private bool roflag = true;
		private WAVETYPE type;
		private Vector3 origin;
		private Vector3 uHat;
		private Vector3 vHat;
		private Vector3 kHat;

		private float eox;
		private float eoy;
		private float theta;

		private float t;
		private float mu;
		private float w;

		private float lambda;
		private float f;
		private float k;

		private float phi;
		private float n;

		private float roDistance;

		public WAVETYPE Type { get => type; set { if (!roflag) type = value; else DebugLogger.Error("WaveParam", "ROFlag Disabled!"); } }
		public Vector3 Origin { get => origin; set { if (!roflag) origin = value; else DebugLogger.Error("WaveParam", "ROFlag Disabled!"); } }
		public Vector3 UHat { get => uHat; set { if (!roflag) uHat = value; else DebugLogger.Error("WaveParam", "ROFlag Disabled!"); } }
		public Vector3 VHat { get => vHat; set { if (!roflag) vHat = value; else DebugLogger.Error("WaveParam", "ROFlag Disabled!"); } }
		public Vector3 KHat { get => kHat; set { if (!roflag) kHat = value; else DebugLogger.Error("WaveParam", "ROFlag Disabled!"); } }

		public float Eox { get => eox; set { if (!roflag) eox = value; else DebugLogger.Error("WaveParam", "ROFlag Disabled!"); } }
		public float Eoy { get => eoy; set { if (!roflag) eoy = value; else DebugLogger.Error("WaveParam", "ROFlag Disabled!"); } }
		public float Theta { get => theta; set { if (!roflag) theta = value; else DebugLogger.Error("WaveParam", "ROFlag Disabled!"); } }

		public float T { get => t; set { if (!roflag) t = value; else DebugLogger.Error("WaveParam", "ROFlag Disabled!"); } }
		public float Mu { get => mu; set { if (!roflag) mu = value; else DebugLogger.Error("WaveParam", "ROFlag Disabled!"); } }
		public float W { get => w; set { if (!roflag) w = value; else DebugLogger.Error("WaveParam", "ROFlag Disabled!"); } }

		public float Lambda { get => lambda; set { if (!roflag) lambda = value; else DebugLogger.Error("WaveParam", "ROFlag Disabled!"); } }
		public float F { get => f; set { if (!roflag) f = value; else DebugLogger.Error("WaveParam", "ROFlag Disabled!"); } }
		public float K { get => k; set { if (!roflag) k = value; else DebugLogger.Error("WaveParam", "ROFlag Disabled!"); } }

		public float Phi { get => phi; set { if (!roflag) phi = value; else DebugLogger.Error("WaveParam", "ROFlag Disabled!"); } }
		public float N { get => n; set { if (!roflag) n = value; else DebugLogger.Error("WaveParam", "ROFlag Disabled!"); } }

		public float RODistance { get => roDistance; set { if (!roflag) roDistance = value; else DebugLogger.Error("WaveParam", "ROFlag Disabled!"); } }

		public WaveParam() { }
		public WaveParam(WaveSourceParam wsParam) {
			roflag = false;
			Type = wsParam.Type;

			Origin = wsParam.Origin;
			UHat = wsParam.UHat;
			VHat = wsParam.VHat;
			KHat = wsParam.KHat;

			Eox = wsParam.Eox;
			Eoy = wsParam.Eoy;
			theta = wsParam.theta;

			Lambda = wsParam.lambda;
			N = wsParam.n;
			RODistance = wsParam.RODistance;

			modifyLambda(); 
			Phi = 0;
			roflag = true;
		}
		public WaveParam(WAVETYPE type, Vector3 origin, Vector3 uHat, Vector3 vHat, Vector3 kHat, float eox, float eoy, float theta, float t, float mu, float w, float lambda, float f, float k, float phi, float n, float roDistance) {
			roflag = false;
			Type = type;
			Origin = origin;
			UHat = uHat;
			VHat = vHat;
			KHat = kHat;
			Eox = eox;
			Eoy = eoy;
			this.theta = theta;
			T = t;
			this.Mu = mu;
			this.W = w;
			this.Lambda = lambda;
			this.F = f;
			this.K = k;
			this.Phi = phi;
			this.N = n;
			RODistance = roDistance;
			roflag = true;
		}

		public void modifyLambda() {
			F = 1 / Lambda;
			K = 2 * Mathf.PI * F;
			W = WaveAlgorithm.C * K / N;
			T = 2 * Mathf.PI / W;
			Mu = 1 / T;
		}
	}
}