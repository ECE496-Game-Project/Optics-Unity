using UnityEngine;
using Complex = System.Numerics.Complex;

using CommonUtils;

namespace WaveUtils {
    public static class WaveAlgorithm {
		public static float C = 299792458;
        public static void changeT(WaveParams param) {
			param.mu = 1 / param.T;
			param.w = 2 * Mathf.PI * param.mu;
			param.k = param.w * param.n / C;
			param.f = param.k / (2 * Mathf.PI);
			param.lambda = 1 / param.f;
		}
        public static void changeW(WaveParams param) {
			param.mu = param.w / (2 * Mathf.PI);
			param.T = 1 / param.mu;
			param.k = param.w * param.n / C;
			param.f = param.k / (2 * Mathf.PI);
			param.lambda = 1 / param.f;
		}
		public static void changeLambda(WaveParams param) {
			param.f = 1 / param.lambda;
			param.k = 2 * Mathf.PI * param.f;
			param.w = C * param.k / param.n;
			param.T = 2 * Mathf.PI / param.w;
			param.mu = 1 / param.T;
		}
		public static void changeK(WaveParams param) {
			param.lambda = (2 * Mathf.PI) / param.k;
			param.f = 1 / param.lambda;
			param.w = C * param.k / param.n;
			param.T = 2 * Mathf.PI / param.w;
			param.mu = 1 / param.T;
		}
		public static void changeN(WaveParams param) {
			param.k = param.w * param.n / C;
			param.f = param.k / (2 * Mathf.PI);
			param.lambda = 1 / param.f;
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


		/// <summary>
		/// calculate the phase accumulated by a wave traveling from origin to r 
		/// </summary>
		/// <param name="r"></param>
		/// <param name="K"></param>
		/// <param name="Phi"></param>
		/// <param name="KHat"></param>
		/// <returns>The accumulated phase in degree [0, 360)</returns>
		public static float CalculateTravelAccumulatedPhase(
			Vector3 r, float K, float Phi,
			in del_Vec3ParamVec3Getter KHat)
		{
            float kdotr = Vector3.Dot(KHat(Vector3.zero), r) * K;
            float expCommon = kdotr + Mathf.Deg2Rad * Phi;

			float degree = Mathf.Rad2Deg * expCommon;

			while (degree >= 360) degree -= 360;
			return degree;
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