using UnityEngine;
using System;
using CommonUtils;
using WaveUtils;
using Interfaces;
using ParameterTransfer;
using System.Collections;

namespace GO_Wave {
    public class WaveSource : MonoBehaviour, I_ParameterTransfer {
        #region GLOBAL VARIABLES
        public I_WaveRender WaveDisplay;
        public I_WaveLogic WaveInteract;
        #endregion

        #region PRIVATE VARIABLES
        protected WaveParams m_params;
        // Current Section's Wave Distance
        [SerializeField] protected float m_effectDistance;
        protected ParameterInfoList m_paramInfoList;
        #endregion

        #region GLOBAL METHODS
        public WaveParams Params {
            get { return m_params; }
            set {
                if (m_params != null)
                    DebugLogger.Error(this.name, "Re-Initalize WaveParameter! Break.");
                m_params = value;
            }
        }
        public float EffectDistance {
            get { return m_effectDistance; }
            set { m_effectDistance = value; }
        }
        
        public virtual void ParameterChangeTrigger() {
            // Refresh EffectDistance from ReadOnly Value
            EffectDistance = m_params.RODistance;

            WaveInteract.CleanInteract();
            WaveInteract.Interact();
            WaveDisplay.RefreshDisplay();
        }

        public void WaveClean() {
            WaveInteract.CleanInteract();
            WaveDisplay.CleanDisplay();
        }

        public void WaveParameterGetAll(out WAVETYPE type, out float eox, out float eoy, out float w, out float k, out float n, out float theta, out float phi) {
            type = m_params.Type;
            eox = m_params.Eox;
            eoy = m_params.Eoy;
            w = m_params.w; 
            k = m_params.k;
            n = m_params.n;
            theta = m_params.theta;
            phi = m_params.phi;
        }
        #endregion

        protected void RegisterDirCallback() {
            switch (m_params.Type) {
                case WAVETYPE.PLANE:
                    m_params.UHat = (in Vector3 r) => { return this.transform.right; };
                    m_params.VHat = (in Vector3 r) => { return this.transform.up; };
                    m_params.KHat = (in Vector3 r) => { return this.transform.forward; };
                    break;
                case WAVETYPE.SPHERE:
                    // [TODO][PointWave]: PointWave UVK direction Function
                    m_params.UHat = (in Vector3 r) => { return Vector3.zero; };
                    m_params.VHat = (in Vector3 r) => { return Vector3.zero; };
                    m_params.KHat = (in Vector3 r) => { return Vector3.zero; };
                    break;
                default:
                    break;
            }
        }
        public virtual void RegisterParametersCallback(ParameterInfoList ParameterInfos) {
            var NameTuple = (ParameterInfo<string>)ParameterInfos.SymbolQuickAccess["Name"];
            var EoxTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["UdirAmp"];
            var EoyTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["VdirAmp"];
            var thetaTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["Theta"];
            var TTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["TemperalPeriod"];
            var muTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["TemperalFreq"];
            var wTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["TemperalAngularFreq"];
            var lambdaTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["SpatialPeriod"];
            var fTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["SpatialFreq"];
            var kTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["SpatialAngularFreq"];
            var phiTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["Phi"];
            var nTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["N"];

            NameTuple.Getter = () => { return this.name; };
            EoxTuple.Getter = () => { return m_params.Eox; };
            EoyTuple.Getter = () => { return m_params.Eoy; };
            thetaTuple.Getter = () => { return m_params.theta; };
            TTuple.Getter = () => { return m_params.T; };
            muTuple.Getter = () => { return m_params.mu; };
            wTuple.Getter = () => { return m_params.w; };
            lambdaTuple.Getter = () => { return m_params.lambda; };
            fTuple.Getter = () => { return m_params.f; };
            kTuple.Getter = () => { return m_params.k; };
            phiTuple.Getter = () => { return m_params.phi; };
            nTuple.Getter = () => { return m_params.n; };

            NameTuple.Setter = (evt) => { this.name = evt.newValue; };
            EoxTuple.Setter = (evt) => { m_params.Eox = evt.newValue; ParameterChangeTrigger(); };
            EoyTuple.Setter = (evt) => { m_params.Eoy = evt.newValue; ParameterChangeTrigger(); };
            thetaTuple.Setter = (evt) => { m_params.theta = evt.newValue; ParameterChangeTrigger(); };
            TTuple.Setter = (evt) => { m_params.T = evt.newValue; WaveAlgorithm.changeT(m_params); ParameterChangeTrigger(); };
            muTuple.Setter = (evt) => { DebugLogger.Warning(this.name, "mu Setter should not be use, something wrong."); };
            wTuple.Setter = (evt) => { m_params.w = evt.newValue; WaveAlgorithm.changeW(m_params); ParameterChangeTrigger(); };
            lambdaTuple.Setter = (evt) => { m_params.lambda = evt.newValue; WaveAlgorithm.changeLambda(m_params); ParameterChangeTrigger(); };
            fTuple.Setter = (evt) => { DebugLogger.Warning(this.name, "f Setter should not be use, something wrong."); };
            kTuple.Setter = (evt) => { m_params.k = evt.newValue; WaveAlgorithm.changeK(m_params); ParameterChangeTrigger(); };
            nTuple.Setter = (evt) => { m_params.n = evt.newValue; WaveAlgorithm.changeN(m_params); ParameterChangeTrigger(); };
            phiTuple.Setter = (evt) => { m_params.phi = evt.newValue; ParameterChangeTrigger(); };
        }

        /// <summary>
        /// Script-Generated-WaveSource Requires to Call ManualAwake.
        /// </summary>
        /// <param name="srcWP"> Pre initalized WaveParameter.</param>
        public void _awake(WaveParams srcWP) {
            WaveAlgorithm.changeT(srcWP);
            if (srcWP.Type == WAVETYPE.INVALID)
                DebugLogger.Error(this.name, "SourceWave Parameter Type Invalid! Stop Executing.");
            WaveDisplay = GetComponent<I_WaveRender>();
            if (WaveDisplay == null)
                DebugLogger.Error(this.name, "GameObject Does not contain WaveDisplay! Stop Executing.");
            WaveInteract = GetComponent<I_WaveLogic>();
            if (WaveInteract == null)
                DebugLogger.Error(this.name, "GameObject Does not contain WaveInteract! Stop Executing.");

            /*init ActiveWaveParams*/
            m_params = srcWP;
            RegisterDirCallback();
        }
        
        public void Start() {
            ParameterChangeTrigger();
        }

    }
}