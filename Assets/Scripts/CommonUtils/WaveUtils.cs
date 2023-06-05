using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using ComplexNumber = System.Numerics.Complex;

namespace CommonUtils{
	[System.Serializable]
	public class WaveParam
	{
		//initial position is using the object transform
		[Header("Direction Settinggs")]
		public Vector3 uHat; // Vector x
		public Vector3 vHat; // Vector y
		public Vector3 kHat; //Wave movingDir

		[Header("Magnitude Settings")]
		public float Eox;
		public float Eoy;

		[Header("Frequency Settings")]
		public float w;
		public float k;
		public float n; //??qianli ??n???w?k?

		[Header("Angle Settings in degree")]
		public float theta;
		public float phi;
	}
	
	[System.Serializable]
	public class WavePlate {
		// "Direction Settinggs"
		// same as Wave that hit on to the Waveplate

		public float degree; // reference to xHat, yHat
	}

	public static class waveAlgorithm {
		public static Vector3 GetIrradiance(Vector3 r, float t, WaveParam p) {
			float kdotr = Vector3.Dot(p.kHat, r) * p.k;
			float expCommon = kdotr - Mathf.Deg2Rad * p.w * t + Mathf.Deg2Rad * p.phi;

			float uMag = p.Eox * Mathf.Cos(expCommon);
			float vMag = p.Eoy * Mathf.Cos(expCommon + Mathf.Deg2Rad * p.theta);
			
			return uMag * p.uHat + vMag * p.vHat;
        }
    }
}