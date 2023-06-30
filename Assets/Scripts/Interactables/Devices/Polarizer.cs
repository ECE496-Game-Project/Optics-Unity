using UnityEngine;
using WaveUtils;
using Complex = System.Numerics.Complex;

namespace Devices {
    public class Polarizer : MonoBehaviour {
        [SerializeField] private float _rotDeg;

        public ComplexMatrix2X2 JohnsMatrix {
            get {
                float rotRad = _rotDeg * Mathf.Deg2Rad;
                return new ComplexMatrix2X2(
                    Complex.One * Mathf.Cos(rotRad) * Mathf.Cos(rotRad),
                    Complex.One * Mathf.Sin(rotRad) * Mathf.Cos(rotRad),
                    Complex.One * -Mathf.Sin(rotRad) * Mathf.Cos(rotRad),
                    Complex.One * Mathf.Sin(rotRad) * Mathf.Sin(rotRad)
                );
            }
        }
    }
}