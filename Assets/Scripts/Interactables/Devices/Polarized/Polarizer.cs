//using UnityEngine;
//using WaveUtils;
//using ParameterTransfer;
//using Complex = System.Numerics.Complex;

//namespace GO_Device {
//    public class Polarizer : PolarizedBase {
//        public float RotDeg;
//        public override void RegisterParametersCallback(ParameterInfoList ParameterInfos) {
//            var RotDegTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["RotDeg"];
//            RotDegTuple.Getter = () => { return RotDeg; };
//            RotDegTuple.Default = RotDeg;
//            RotDegTuple.Setter = (evt) => { RotDeg = evt.newValue; ParameterChangeTrigger(); };
//        }

//        public override ComplexMatrix2X2 JohnsMatrix {
//            get {
//                float rotRad = RotDeg * Mathf.Deg2Rad;
//                return new ComplexMatrix2X2(
//                    Complex.One * Mathf.Cos(rotRad) * Mathf.Cos(rotRad),
//                    Complex.One * Mathf.Sin(rotRad) * Mathf.Cos(rotRad),
//                    Complex.One * Mathf.Sin(rotRad) * Mathf.Cos(rotRad),
//                    Complex.One * Mathf.Sin(rotRad) * Mathf.Sin(rotRad)
//                );
//            }
//        }
//    }
//}