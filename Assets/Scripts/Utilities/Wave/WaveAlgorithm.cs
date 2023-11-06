using UnityEngine;
using Complex = System.Numerics.Complex;

using CommonUtils;

namespace WaveUtils {
    public static class WaveAlgorithm {
		public static float C = 299792458;
        public static void changeT(in float T, out float mu, out float w, out float lambda, out float f, out float k, in float n) {
			mu = 1 / T;
			w = 2 * Mathf.PI * mu;
			k = w * n / C;
			f = k / (2 * Mathf.PI);
			lambda = 1 / f;
		}
        public static void changeW(out float T, out float mu, in float w, out float lambda, out float f, out float k, in float n) {
			mu = w / (2 * Mathf.PI);
			T = 1 / mu;
			k = w * n / C;
			f = k / (2 * Mathf.PI);
			lambda = 1 / f;
		}
		public static void changeLambda(out float T, out float mu, out float w, in float lambda, out float f, out float k, in float n) {
			f = 1 / lambda;
			k = 2 * Mathf.PI * f;
			w = C * k / n;
			T = 2 * Mathf.PI / w;
			mu = 1 / T;
		}
		public static void changeK(out float T, out float mu, out float w, out float lambda, out float f, in float k, in float n) {
			lambda = (2 * Mathf.PI) / k;
			f = 1 / lambda;
			w = C * k / n;
			T = 2 * Mathf.PI / w;
			mu = 1 / T;
		}
		public static void changeN(in float w, out float lambda, out float f, out float k, in float n) {
			k = w * n / C;
			f = k / (2 * Mathf.PI);
			lambda = 1 / f;
		}

		public static Vector3 CalcIrradiance(
			Vector3 r, float t, 
			in float Eox, in float Eoy,
			in float W, in float K, in float N,
			in float Theta, in float Phi,
			in del_Vec3ParamVec3Getter UHat,
			in del_Vec3ParamVec3Getter VHat,
			in del_Vec3ParamVec3Getter KHat
		) {
			float kdotr = Vector3.Dot(KHat(Vector3.zero), r) * K;
			float expCommon = kdotr - Mathf.Deg2Rad * W * t + Mathf.Deg2Rad * Phi;

			float uMag = Eox * Mathf.Cos(expCommon);
			float vMag = Eoy * Mathf.Cos(expCommon + Mathf.Deg2Rad * Theta);
			
			return uMag * UHat(Vector3.zero) + vMag * VHat(Vector3.zero);
        }
		public static void WaveToJohnsVector(in float Eox, in float Eoy, in float Theta, out ComplexVector2 cv) {
			cv = new ComplexVector2(
				new Complex(Eox, 0),
				Eoy * Complex.Exp(Complex.ImaginaryOne * Mathf.Deg2Rad * Theta)
			);
		}

		public static void JohnsVectorToWave(in ComplexVector2 cv, out float Eox, out float Eoy, out float Theta) {
			if (cv == null) {
				DebugLogger.Error("JohnsVectorToWave", "Pass in NULL class, Error.");
				Eox = Eoy = Theta = 0;
				return;
			}
			Theta = (float)(Complex.Log(cv.Value[1]).Imaginary - Complex.Log(cv.Value[0]).Imaginary) * Mathf.Rad2Deg;

			Eox = (float)(cv.Value[0].Magnitude);
			Eoy = (float)(cv.Value[1].Magnitude);
		}
    }
}