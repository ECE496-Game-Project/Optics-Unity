using System.Collections.Generic;
using UnityEngine;
using Complex = System.Numerics.Complex;

namespace WaveUtils {
    public static class WaveAlgorithm {
		public static Vector3 CalcIrradiance(Vector3 r, float t, WaveParams p) {
			float kdotr = Vector3.Dot(p.KHat(Vector3.zero), r) * p.K;
			float expCommon = kdotr - Mathf.Deg2Rad * p.W * t + Mathf.Deg2Rad * p.Phi;

			float uMag = p.Eox * Mathf.Cos(expCommon);
			float vMag = p.Eoy * Mathf.Cos(expCommon + Mathf.Deg2Rad * p.Theta);
			
			return uMag * p.UHat(Vector3.zero) + vMag * p.VHat(Vector3.zero);
        }
		public static void WaveToJohnsVector(in WaveParams wp, ref ComplexVector2 cv) {
			cv = new ComplexVector2(
				new Complex(wp.Eox, 0),
				wp.Eoy * Complex.Exp(Complex.ImaginaryOne * Mathf.Deg2Rad * wp.Theta)
			);
		}

		public static void JohnsVectorToWave(in ComplexVector2 cv, ref WaveParams wp) {

        }
    }
}