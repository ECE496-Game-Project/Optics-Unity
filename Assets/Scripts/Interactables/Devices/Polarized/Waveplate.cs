using System.Collections;
using UnityEngine;
using WaveUtils;
using Complex = System.Numerics.Complex;

namespace GO_Device {
    public class Waveplate : PolarizedBase {
        [SerializeField] private float _plateDeg;
        [SerializeField] private float _axisDiffDeg;

        public override ComplexMatrix2X2 JohnsMatrix {
            get {
                Complex axisdiff = Complex.Exp(Complex.ImaginaryOne * _axisDiffDeg * Mathf.Deg2Rad);
                float plateRad = _plateDeg * Mathf.Deg2Rad;
                float cos2PlateDeg = Mathf.Cos(plateRad) * Mathf.Cos(plateRad);
                float sin2PlateDeg = Mathf.Sin(plateRad) * Mathf.Sin(plateRad);
                float sincosPlateDeg = Mathf.Sin(plateRad) * Mathf.Cos(plateRad);

                return new ComplexMatrix2X2(
                    axisdiff * cos2PlateDeg + sin2PlateDeg * Complex.One,
                    axisdiff * sincosPlateDeg - sincosPlateDeg * Complex.One,
                    axisdiff * sincosPlateDeg - sincosPlateDeg * Complex.One,
                    axisdiff * sin2PlateDeg - sincosPlateDeg * Complex.One
                );
            }
        }
    }
}