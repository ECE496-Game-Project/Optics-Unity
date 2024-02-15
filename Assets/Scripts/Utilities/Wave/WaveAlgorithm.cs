using UnityEngine;
using Complex = System.Numerics.Complex;
using CommonUtils;

namespace WaveUtils {
    public static class WaveAlgorithm {
        public static float C = 299.792458f; // Unit is nm/fs
        public static void changeT(WaveParam param) {
            param.mu = 1 / param.T;
            param.w = 2 * Mathf.PI * param.mu;
            param.k = param.w * param.n / C;
            param.f = param.k / (2 * Mathf.PI);
            param.lambda = 1 / param.f;
        }
        public static void changeW(WaveParam param) {
            param.mu = param.w / (2 * Mathf.PI);
            param.T = 1 / param.mu;
            param.k = param.w * param.n / C;
            param.f = param.k / (2 * Mathf.PI);
            param.lambda = 1 / param.f;
        }
        public static void changeMu(WaveParam param) {
            param.w = param.mu * 2 * Mathf.PI;
            param.T = 1 / param.mu;
            param.k = param.w * param.n / C;
            param.f = param.k / (2 * Mathf.PI);
            param.lambda = 1 / param.f;
        }
        public static void changeLambda(WaveParam param) {
            param.f = 1 / param.lambda;
            param.k = 2 * Mathf.PI * param.f;
            param.w = C * param.k / param.n;
            param.T = 2 * Mathf.PI / param.w;
            param.mu = 1 / param.T;
        }
        public static void changeK(WaveParam param) {
            param.lambda = (2 * Mathf.PI) / param.k;
            param.f = 1 / param.lambda;
            param.w = C * param.k / param.n;
            param.T = 2 * Mathf.PI / param.w;
            param.mu = 1 / param.T;
        }
        public static void changeN(WaveParam param) {
            param.k = param.w * param.n / C;
            param.f = param.k / (2 * Mathf.PI);
            param.lambda = 1 / param.f;
        }

        public static Vector3 CalcIrradiance(Vector3 currPos, float t, WaveParam param) {
            Vector3 r = param.Origin - currPos;
			float kdotr = Vector3.Dot(param.KHat, r) * param.k;
			float expCommon = kdotr - Mathf.Deg2Rad * param.w * t + Mathf.Deg2Rad * param.phi;

			float uMag = param.Eox * Mathf.Cos(expCommon);
			float vMag = param.Eoy * Mathf.Cos(expCommon + Mathf.Deg2Rad * param.theta);
			
			return uMag * param.UHat + vMag * param.VHat;
        }

        /// <summary>
        /// calculate the phase accumulated by a wave traveling from origin to r,
		/// assign it to outputParam.phi
        /// </summary>
        /// <param name="r"></param>
        /// <param name="K"></param>
        /// <param name="Phi"></param>
        /// <param name="KHat"></param>
        /// <returns>The accumulated phase in degree [0, 360)</returns>
        public static float CalculateTravelAccumulatedPhase(
            Vector3 r, WaveParam InputParam, WaveParam outputParam)
        {
            float kdotr = Vector3.Dot(InputParam.KHat, r) * InputParam.k;
            float expCommon = kdotr + Mathf.Deg2Rad * InputParam.phi;

            float degree = Mathf.Rad2Deg * expCommon;

            while (degree >= 360) degree -= 360;

            outputParam.phi = degree;
            return degree;
        }

        public static ComplexVector2 WaveToJohnsVector(WaveParam param) {
			return new ComplexVector2(
				new Complex(param.Eox, 0),
				param.Eoy * Complex.Exp(Complex.ImaginaryOne * Mathf.Deg2Rad * param.theta)
			);
		}

		public static void JohnsVectorToWave(in ComplexVector2 cv, out float Eox, out float Eoy, ref float phi, out float theta) {

            if (cv == null) {
                DebugLogger.Error("JohnsVectorToWave", "Pass in NULL class, Error.");
                Eox = Eoy = theta = 0;
                return;
            }

			float accumulatedPhase = 0;
			if (cv.Value[0].Magnitude == 0) {
                accumulatedPhase = (float)Complex.Log(cv.Value[1]).Imaginary * Mathf.Rad2Deg;
                theta = 0;
            } else if (cv.Value[1].Magnitude == 0) {
                accumulatedPhase = (float)Complex.Log(cv.Value[0]).Imaginary * Mathf.Rad2Deg;
                theta = 0;
            } else {
                accumulatedPhase = (float)Complex.Log(cv.Value[0]).Imaginary * Mathf.Rad2Deg;
                theta = (float)(Complex.Log(cv.Value[1]).Imaginary - Complex.Log(cv.Value[0]).Imaginary) * Mathf.Rad2Deg;
            }

			phi += accumulatedPhase;

			while (phi >= 360) phi -= 360;

            if (theta < 0) theta += 360;

            Eox = (float)(cv.Value[0].Magnitude);
            Eoy = (float)(cv.Value[1].Magnitude);
		}
    }
}