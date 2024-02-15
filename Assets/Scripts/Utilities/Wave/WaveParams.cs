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

		[Header("Temporial Frequency(THz)")]
		public float mu;

		[Header("Light Transimitting Material's Refractive Index")]
		[Range(1, 5)]
		public float n;

		[Header("Dispersion Distance")]
		public float RODistance;
		#endregion
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

			mu = wsParam.mu;
			n = wsParam.n;
			RODistance = wsParam.RODistance;

			// For fields without direct equivalents in WaveSourceParam, we need to initialize them.
			// Here, I'm calling modifyMu() as an example to calculate related fields based on mu and n,
			// assuming mu is a defining property. Adjust based on your application's logic.
			modifyMu(); // This sets T, w, k, f, lambda based on mu and n. Adjust as necessary.

			phi = 0; // Assuming a default value for phi if it's not provided by WaveSourceParam
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

		public static float C = 299.792458f; // Unit is nm/fs
		public void modifyT() {
			mu = 1 / T;
			w = 2 * Mathf.PI * mu;
			k = w * n / C;
			f = k / (2 * Mathf.PI);
			lambda = 1 / f;
		}
		public void modifyW() {
			mu = w / (2 * Mathf.PI);
			T = 1 / mu;
			k = w * n / C;
			f = k / (2 * Mathf.PI);
			lambda = 1 / f;
		}
		public void modifyMu() {
			w = mu * 2 * Mathf.PI;
			T = 1 / mu;
			k = w * n / C;
			f = k / (2 * Mathf.PI);
			lambda = 1 / f;
		}
		public void modifyLambda() {
			f = 1 / lambda;
			k = 2 * Mathf.PI * f;
			w = C * k / n;
			T = 2 * Mathf.PI / w;
			mu = 1 / T;
		}
		public void modifyK() {
			lambda = (2 * Mathf.PI) / k;
			f = 1 / lambda;
			w = C * k / n;
			T = 2 * Mathf.PI / w;
			mu = 1 / T;
		}
		public void modifyN() {
			k = w * n / C;
			f = k / (2 * Mathf.PI);
			lambda = 1 / f;
		}
	}
}