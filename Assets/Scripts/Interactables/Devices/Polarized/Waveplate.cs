using System;
using UnityEngine;
using WaveUtils;
using Interfaces;
using Complex = System.Numerics.Complex;

namespace GO_Device {
    public class Waveplate : PolarizedBase, I_ParameterTransfer {
        public float PlateDeg;
        public float AxisDiffDeg;

        public override bool ParameterSet<T>(string paramName, T value) {
            if(paramName == "PlateDeg") {
                PlateDeg = (float)Convert.ToDouble(value);
            }
            else if (paramName == "AxisDiffDeg") {
                AxisDiffDeg = (float)Convert.ToDouble(value);
            }
            else {
                return base.ParameterSet(paramName, value);
            }
            //foreach(var pair in m_childParentPair) {
            //    WaveClean(pair.Value);
            //    pair.Key.ParamChangeTrigger();
            //}
            return true;
        }
        public override T ParameterGet<T>(string paramName) {
            if (paramName == "PlateDeg") {
                return (T)(object)PlateDeg;
            }
            else if (paramName == "AxisDiffDeg") {
                return (T)(object)AxisDiffDeg;
            }
            else {
                return base.ParameterGet<T>(paramName);
            }
        }

        public override ComplexMatrix2X2 JohnsMatrix {
            get {
                Complex axisdiff = Complex.Exp(Complex.ImaginaryOne * AxisDiffDeg * Mathf.Deg2Rad);
                float plateRad = PlateDeg * Mathf.Deg2Rad;
                float cos2PlateDeg = Mathf.Cos(plateRad) * Mathf.Cos(plateRad);
                float sin2PlateDeg = Mathf.Sin(plateRad) * Mathf.Sin(plateRad);
                float sincosPlateDeg = Mathf.Sin(plateRad) * Mathf.Cos(plateRad);

                return new ComplexMatrix2X2(
                    axisdiff * cos2PlateDeg + sin2PlateDeg * Complex.One,
                    axisdiff * sincosPlateDeg - sincosPlateDeg * Complex.One,
                    axisdiff * sincosPlateDeg - sincosPlateDeg * Complex.One,
                    axisdiff * sin2PlateDeg + cos2PlateDeg * Complex.One
                );
            }
        }
    }
}