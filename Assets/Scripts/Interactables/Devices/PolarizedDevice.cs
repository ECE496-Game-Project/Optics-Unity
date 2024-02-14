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

    public class PolarizedDevice : DeviceBase {
        public override string ParamTransferName { get { return "PolarizedDevice"; } }

        [SerializeField] protected DEVICETYPE DeviceType;
        [SerializeField] private float ThicknessOffset;
        [SerializeField] private float RotDeg;
        [SerializeField] private float AxisDiffDeg;

        private WaveSource m_parent;
        private WaveSource m_child;

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

        public override void RegisterParametersCallback(ParameterInfoList ParameterInfos) {
            var NameTuple = (ParameterInfo<string>)ParameterInfos.SymbolQuickAccess["Name"];
            var DeviceTypeTuple = (ParameterInfo<Enum>)ParameterInfos.SymbolQuickAccess["DeviceType"];
            var RotDegTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["RotDeg"];
            var AxisDiffDegTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["AxisDiffDeg"];

            NameTuple.Getter = () => { return this.name; };
            DeviceTypeTuple.Getter = () => { return DeviceType; };
            RotDegTuple.Getter = () => { return RotDeg; };
            AxisDiffDegTuple.Getter = () => { return AxisDiffDeg; };

            NameTuple.Setter = (evt) => { /*this.name = evt.newValue;*/ };
            DeviceTypeTuple.Setter = (evt) => { DeviceType = (DEVICETYPE)evt.newValue; ParameterChangeTrigger(); };
            RotDegTuple.Setter = (evt) => { RotDeg = evt.newValue; ParameterChangeTrigger(); };
            AxisDiffDegTuple.Setter = (evt) => { AxisDiffDeg = evt.newValue; ParameterChangeTrigger(); };
        }

        public override void ParameterChangeTrigger() {
            m_parent.ParameterChangeTrigger();
        }

        public override void WaveHit(in RaycastHit hit, WaveSource parentWS) {
            /*GO Setup*/
            // Awake function in scripts will be called in the order of,
            // WaveSource -> LineWaveRender -> BoxCollider -> LineWaveLogic
            GameObject new_GO = new GameObject(
                parentWS.name + "_Child", 
                typeof(WaveSource), 
                typeof(LineWaveRender), 
                typeof(BoxCollider), 
                typeof(LineWaveLogic),
                typeof(SelectableChildWave)
            );
            new_GO.transform.position = hit.point + Vector3.Normalize(hit.point - parentWS.transform.position) * ThicknessOffset;
            new_GO.transform.rotation = parentWS.transform.rotation;

            /*Wave Source, Display, Interact Setup*/
            WaveSource childWS = new_GO.GetComponent<WaveSource>();
            LineWaveRender lwd = new_GO.GetComponent<LineWaveRender>();
            LineWaveLogic lwi = new_GO.GetComponent<LineWaveLogic>();

            WaveParams childWP = new WaveParams();
            /* Calculate Eox, Eoy, Theta*/
            ComplexVector2 resVec = WaveAlgorithm.WaveToJohnsVector(parentWS.Params);
            
            if(DeviceType == DEVICETYPE.POLARIZER)
                resVec = PolarizerMatrix() * resVec;
            else if(DeviceType == DEVICETYPE.WEAVEPLATE)
            {
                resVec = WaveplateMatrix() * resVec;
            }

            /* Calculate ReadOnly Effective Distance*/
            LineWaveLogic parentLWL = (LineWaveLogic)parentWS.WaveLogic;
            float tmpDistance = parentLWL.EffectDistance;
            parentLWL.EffectDistance = hit.distance;
            childWP.RODistance = tmpDistance - hit.distance;

            /* Copy Parent's t & n, then compute rest*/
            childWP.T = parentWS.Params.T;
            childWP.n = parentWS.Params.n;

            /* childWS type must be Plane*/
            childWP.Type = WAVETYPE.PLANE;

            childWS._awake(childWP);
            

            WaveAlgorithm.CalculateTravelAccumulatedPhase(hit.point - parentWS.transform.position, parentWS.Params, childWS.Params);
            WaveAlgorithm.JohnsVectorToWave(resVec, childWP);

            lwd.init(parentWS.WaveDisplay);
            lwi.init(parentWS.WaveLogic);

            
            /*Store Pair*/
            m_parent = parentWS;
            m_child = childWS;
        }

        public override void CleanDeviceHitTrace(WaveSource parentWS) {
            if (m_parent == null && m_child == null) return;
            m_child.WaveClean();
            Destroy(m_child.gameObject);
            m_child = null;
            m_parent = null;
        }

        public void Awake() {
            m_child = null;
            m_parent = null;
        }
    }
}