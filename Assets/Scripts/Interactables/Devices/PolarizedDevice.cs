using System;
using UnityEngine;
using GO_Wave;
using WaveUtils;
using ParameterTransfer;
using CommonUtils;
using SelectItems;
using Complex = System.Numerics.Complex;

namespace GO_Device {
    public enum DEVICETYPE {
        POLARIZER,
        WEAVEPLATE,
    }

    public partial class PolarizedDevice : DeviceBase {

        [SerializeField] protected DEVICETYPE DeviceType;
        [SerializeField] private float ThicknessOffset;
        [SerializeField] private float RotDeg;
        [SerializeField] private float AxisDiffDeg;

        [SerializeField] private Wave m_parent = null;
        [SerializeField] private Wave m_child = null;

        public ComplexMatrix2X2 PolarizerMatrix() {
            if(AxisDiffDeg != 0)
                DebugLogger.Warning(this.name, "slow Fast Axis define in Polarizer!");

            float rotRad = RotDeg * Mathf.Deg2Rad;
            return new ComplexMatrix2X2(
                Complex.One * Mathf.Cos(rotRad) * Mathf.Cos(rotRad),
                Complex.One * Mathf.Sin(rotRad) * Mathf.Cos(rotRad),
                Complex.One * Mathf.Sin(rotRad) * Mathf.Cos(rotRad),
                Complex.One * Mathf.Sin(rotRad) * Mathf.Sin(rotRad)
            );
        }
        public ComplexMatrix2X2 WaveplateMatrix() {
            Complex axisdiff = Complex.Exp(Complex.ImaginaryOne * AxisDiffDeg * Mathf.Deg2Rad);
            float plateRad = RotDeg * Mathf.Deg2Rad;
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

        public override void WaveHit(in RaycastHit hit, Wave parentWave) {
            /*GO Setup*/
            WaveParam parentWP = parentWave.Params;
            LineWaveLogic parentLWL = (LineWaveLogic)parentWave.WaveLogic;
            LineWaveDisplay parentLWD = (LineWaveDisplay)parentWave.WaveDisplay;

            /* Calculate Eox, Eoy, Theta*/
            ComplexVector2 resVec = WaveAlgorithm.WaveToJohnsVector(parentWave.Params);

            if (DeviceType == DEVICETYPE.POLARIZER)
                resVec = PolarizerMatrix() * resVec;
            else if (DeviceType == DEVICETYPE.WEAVEPLATE) {
                resVec = WaveplateMatrix() * resVec;
            }
            float eox, eoy, phi = parentWP.Phi, theta;
            WaveAlgorithm.JohnsVectorToWave(resVec, out eox, out eoy, ref phi, out theta);

            /* Calculate ReadOnly Effective Distance*/
            float tmpDistance = parentLWL.EffectDistance;
            parentLWL.EffectDistance = hit.distance;

            var childWP = new WaveParam(
                parentWP.Type, parentWP.Origin,
                parentWP.UHat, parentWP.VHat, parentWP.KHat,
                eox, eoy, theta,
                parentWP.T, parentWP.Mu, parentWP.W, parentWP.Lambda, parentWP.F, parentWP.K, 
                phi, parentWP.N,
                tmpDistance - hit.distance
            );

            Wave childWave = Wave.NewLineWave(
                this.name + "GenLineWave",
                parentWave.correspondWS, childWP, 
                parentLWL.InteractMask, 
                parentLWD.SampleResolution,
                hit.point + Vector3.Normalize(hit.point - parentWave.transform.position) * ThicknessOffset,
                parentWave.transform.rotation
            );
            /*Store Pair*/
            m_parent = parentWave;
            m_child = childWave;
            correspondWS = m_parent.correspondWS;
        }

        public override void CleanDeviceHitTrace(Wave parentWS) {
            m_child = null;
            m_parent = null;
            correspondWS = null;
        }
    }
}