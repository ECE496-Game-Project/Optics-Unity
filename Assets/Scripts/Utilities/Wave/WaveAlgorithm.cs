using UnityEngine;
using Complex = System.Numerics.Complex;

using CommonUtils;

namespace WaveUtils {
    public static class WaveAlgorithm {
		public static Vector3 CalcIrradiance(Vector3 r, float t, WaveParams p) {
			float kdotr = Vector3.Dot(p.KHat(Vector3.zero), r) * p.K;
			float expCommon = kdotr - Mathf.Deg2Rad * p.W * t + Mathf.Deg2Rad * p.Phi;

			float uMag = p.Eox * Mathf.Cos(expCommon);
			float vMag = p.Eoy * Mathf.Cos(expCommon + Mathf.Deg2Rad * p.Theta);
			
			return uMag * p.UHat(Vector3.zero) + vMag * p.VHat(Vector3.zero);
        }
		public static void WaveToJohnsVector(WaveParams wp, ComplexVector2 cv) {
			if (cv == null || wp == null) {
				DebugLogger.Error("WaveToJohnsVector", "Pass in NULL class, Error.");
				return;
			}
			cv.Value[0] = new Complex(wp.Eox, 0);
			cv.Value[1] = wp.Eoy * Complex.Exp(Complex.ImaginaryOne * Mathf.Deg2Rad * wp.Theta);
		}

		public static void JohnsVectorToWave(ComplexVector2 cv, WaveParams wp) {
			if (cv == null || wp == null) {
				DebugLogger.Error("JohnsVectorToWave", "Pass in NULL class, Error.");
				return;
			}
			wp.Theta = (float)(Complex.Log(cv.Value[0]).Imaginary + Complex.Log(cv.Value[1]).Imaginary);

			wp.Eox = (float)(cv.Value[0].Magnitude);
			wp.Eoy = (float)(cv.Value[1].Magnitude);
		}
    }
}