using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Complex = System.Numerics.Complex;

namespace CommonUtils {
	[System.Serializable]
	public class WaveParam {
		[Header("Direction Settinggs")]
		public Vector3 UHat; // Vector x
		public Vector3 VHat; // Vector y
		public Vector3 KHat; //Wave movingDir
		public Vector3 Origin;

		[Header("Magnitude Settings")]
		public float Eox;
		public float Eoy;

		[Header("Frequency Settings")]
		public float W;
		public float K;
		public float N; //??qianli ??n???w?k?

		[Header("Angle Settings in degree")]
		[Range(0, 360)]
		public float Theta;
		[Range(0, 360)]
		public float Phi;

		public ComplexVector2 JohnsVector {
			get {
				return WaveAlgorithm.CalcJohnsVector(Eox, Eoy, Theta);
			}
		}

		public WaveParam(
			Vector3 uHat, Vector3 vHat, Vector3 kHat, 
			Vector3 origin, float Eox, float Eoy, 
			float w, float k, float n, 
			float theta, float phi) 
		{
            this.UHat = uHat;
            this.VHat = vHat;
            this.KHat = kHat;
            this.Origin = origin;
			this.Eox = Eox;
			this.Eoy = Eoy;
            this.W = w;
            this.K = k;
            this.N = n;
            this.Theta = theta;
            this.Phi = phi;
        }

		public WaveParam(WaveParam src) {
			this.UHat = src.UHat;
			this.VHat = src.VHat;
			this.KHat = src.KHat;
			this.Origin = src.Origin;
			this.Eox = src.Eox;
			this.Eoy = src.Eoy;
			this.W = src.W;
			this.K = src.K;
			this.N = src.N;
			this.Theta = src.Theta;
			this.Phi = src.Phi;
		}
    }

	public static class WaveAlgorithm {
		public static Vector3 CalcIrradiance(Vector3 r, float t, WaveParam p) {
			float kdotr = Vector3.Dot(p.KHat, r) * p.K;
			float expCommon = kdotr - Mathf.Deg2Rad * p.W * t + Mathf.Deg2Rad * p.Phi;

			float uMag = p.Eox * Mathf.Cos(expCommon);
			float vMag = p.Eoy * Mathf.Cos(expCommon + Mathf.Deg2Rad * p.Theta);
			
			return uMag * p.UHat + vMag * p.VHat;
        }
		public static ComplexVector2 CalcJohnsVector(float Eox, float Eoy, float theta) {
			return new ComplexVector2(
				new Complex(Eox, 0),
				Eoy * Complex.Exp(Complex.ImaginaryOne * Mathf.Deg2Rad * theta)
			);
		}

		//TODO: JohnsVector to Eox Eoy theta
    }
}