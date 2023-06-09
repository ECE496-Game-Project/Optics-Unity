using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Complex = System.Numerics.Complex;
using Accord.Math;

namespace Wave {
	[System.Serializable]
	public class WaveParam
	{
		[Header("Direction Settinggs")]
		public Vector3 uHat; // Vector x
		public Vector3 vHat; // Vector y
		public Vector3 kHat; //Wave movingDir
		public Vector3 origin;

		[Header("Magnitude Settings")]
		public float Eox;
		public float Eoy;

		[Header("Frequency Settings")]
		public float w;
		public float k;
		public float n; //??qianli ??n???w?k?

		[Header("Angle Settings in degree")]
        [Range(0, 360)]
		public float theta;
		[Range(0, 360)]
		public float phi;

		/// <summary>
		/// Calculated using uHat, vHat, kHat, and phi
		/// </summary>
		[HideInInspector] public Complex[] JohnsVector2;
	}
	
	[System.Serializable]
	public class WavePlate {
		// "Direction Settinggs"
		// same as Wave that hit on to the Waveplate

		public float degree; // reference to xHat, yHat

		public Complex[,] JohnsMatrix2X2;
	}

	public static class WaveAlgorithm {
		public static Vector3 CalcIrradiance(Vector3 r, float t, WaveParam p) {
			float kdotr = Vector3.Dot(p.kHat, r) * p.k;
			float expCommon = kdotr - Mathf.Deg2Rad * p.w * t + Mathf.Deg2Rad * p.phi;

			float uMag = p.Eox * Mathf.Cos(expCommon);
			float vMag = p.Eoy * Mathf.Cos(expCommon + Mathf.Deg2Rad * p.theta);
			
			return uMag * p.uHat + vMag * p.vHat;
        }
		public static void CalcJohnsVector(WaveParam p) {

        }
		
    }
}