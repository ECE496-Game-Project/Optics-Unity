using UnityEngine;
using WaveUtils;
using System;
using Complex = System.Numerics.Complex;

namespace GO_Device {
    public class Polarizer : PolarizedBase {
        public float RotDeg;
        public override bool ParameterSet<T>(string paramName, T value) {
            if(paramName == "RotDeg") {
                RotDeg = (float)Convert.ToDouble(value);
                //foreach (var pair in m_childParentPair) {
                //    WaveClean(pair.Value);
                //    pair.Key.ParamChangeTrigger();
                //}
                return true;
            }
            else return base.ParameterSet(paramName, value);

        }
        public override T ParameterGet<T>(string paramName) {
            if (paramName == "RotDeg") {
                return (T)(object)RotDeg;
            }
            else {
                return base.ParameterGet<T>(paramName);
            }
        }
        public override ComplexMatrix2X2 JohnsMatrix {
            get {
                float rotRad = RotDeg * Mathf.Deg2Rad;
                return new ComplexMatrix2X2(
                    Complex.One * Mathf.Cos(rotRad) * Mathf.Cos(rotRad),
                    Complex.One * Mathf.Sin(rotRad) * Mathf.Cos(rotRad),
                    Complex.One * Mathf.Sin(rotRad) * Mathf.Cos(rotRad),
                    Complex.One * Mathf.Sin(rotRad) * Mathf.Sin(rotRad)
                );
            }
        }
    }
}