//using UnityEngine;
//using WaveUtils;
//using ParameterTransfer;
//using Complex = System.Numerics.Complex;

//namespace GO_Device {
//    public class Waveplate : PolarizedBase {
//        public float PlateDeg;
//        public float AxisDiffDeg;

//        public override void RegisterParametersCallback(ParameterInfoList ParameterInfos) {
//            var PlateDegTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["PlateDeg"];
//            var AxisDiffDegTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["AxisDiffDeg"];
//            PlateDegTuple.Getter = () => { return PlateDeg; };
//            AxisDiffDegTuple.Getter = () => { return AxisDiffDeg; };
//            PlateDegTuple.Default = PlateDeg;
//            AxisDiffDegTuple.Default = AxisDiffDeg;
//            PlateDegTuple.Setter = (evt) => { PlateDeg = evt.newValue; ParameterChangeTrigger(); };
//            AxisDiffDegTuple.Setter = (evt) => { AxisDiffDeg = evt.newValue; ParameterChangeTrigger(); };
//        }

//        public override ComplexMatrix2X2 JohnsMatrix {
//            get {
//                Complex axisdiff = Complex.Exp(Complex.ImaginaryOne * AxisDiffDeg * Mathf.Deg2Rad);
//                float plateRad = PlateDeg * Mathf.Deg2Rad;
//                float cos2PlateDeg = Mathf.Cos(plateRad) * Mathf.Cos(plateRad);
//                float sin2PlateDeg = Mathf.Sin(plateRad) * Mathf.Sin(plateRad);
//                float sincosPlateDeg = Mathf.Sin(plateRad) * Mathf.Cos(plateRad);

//                return new ComplexMatrix2X2(
//                    axisdiff * cos2PlateDeg + sin2PlateDeg * Complex.One,
//                    axisdiff * sincosPlateDeg - sincosPlateDeg * Complex.One,
//                    axisdiff * sincosPlateDeg - sincosPlateDeg * Complex.One,
//                    axisdiff * sin2PlateDeg + cos2PlateDeg * Complex.One
//                );
//            }
//        }
//    }
//}