using UnityEngine;
using Complex = System.Numerics.Complex;

using CommonUtils;

namespace WaveUtils {
    public static class WaveAlgorithm {
		public static Vector3 CalcIrradiance(Vector3 r, float t, WaveParams p) {
			float kdotr = Vector3.Dot(p.KHat(Vector3.zero), r) * p.K.Value;
			float expCommon = kdotr - Mathf.Deg2Rad * p.W.Value * t + Mathf.Deg2Rad * p.Phi.Value;

			float uMag = p.Eox.Value * Mathf.Cos(expCommon);
			float vMag = p.Eoy.Value * Mathf.Cos(expCommon + Mathf.Deg2Rad * p.Theta.Value);
			
			return uMag * p.UHat(Vector3.zero) + vMag * p.VHat(Vector3.zero);
        }
		public static void WaveToJohnsVector(WaveParams wp, ComplexVector2 cv) {
			if (cv == null || wp == null) {
				DebugLogger.Error("WaveToJohnsVector", "Pass in NULL class, Error.");
				return;
			}
			cv.Value[0] = new Complex(wp.Eox.Value, 0);
			cv.Value[1] = wp.Eoy.Value * Complex.Exp(Complex.ImaginaryOne * Mathf.Deg2Rad * wp.Theta.Value);
		}

		public static void JohnsVectorToWave(ComplexVector2 cv, WaveParams wp) {
			if (cv == null || wp == null) {
				DebugLogger.Error("JohnsVectorToWave", "Pass in NULL class, Error.");
				return;
			}
			wp.Theta.Value = (float)(Complex.Log(cv.Value[1]).Imaginary - Complex.Log(cv.Value[0]).Imaginary) * Mathf.Rad2Deg;

			wp.Eox.Value = (float)(cv.Value[0].Magnitude);
			wp.Eoy.Value = (float)(cv.Value[1].Magnitude);
		}
    }
}