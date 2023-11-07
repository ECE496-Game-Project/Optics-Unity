using UnityEngine;
using System;
using CommonUtils;
using WaveUtils;
using Interfaces;
using ParameterTransfer;

namespace GO_Wave {
    public class WaveSource : MonoBehaviour, I_ParameterTransfer {
        #region GLOBAL VARIABLES
        public I_WaveRender WaveDisplay;
        public I_WaveLogic WaveInteract;
        #endregion

#region PRIVATE VARIABLES
        [Header("DEBUG_WAVE")]
        [SerializeField] protected WaveParams m_params;
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
            // Child Wave Parameter Registration
            var NameTuple = (ParameterInfo<string>)ParameterInfos.SymbolQuickAccess["name"];
            var EoxTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["Eox"];
            var EoyTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["Eoy"];
            var thetaTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["theta"];
            var TTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["T"];
            var muTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["mu"];
            var wTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["w"];
            var lambdaTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["lambda"];
            var fTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["f"];
            var kTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["k"];
            var phiTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["phi"];
            var nTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["n"];

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

            NameTuple.Default = this.name;
            EoxTuple.Default = m_params.Eox;
            EoyTuple.Default = m_params.Eoy;
            thetaTuple.Default = m_params.theta;
            TTuple.Default = m_params.T;
            muTuple.Default = m_params.mu;
            wTuple.Default = m_params.w;
            lambdaTuple.Default = m_params.lambda;
            fTuple.Default = m_params.f;
            kTuple.Default = m_params.k;
            phiTuple.Default = m_params.phi;
            nTuple.Default = m_params.n;

            NameTuple.Setter = (evt) => { this.name = evt.newValue; };
        }
        /// <summary>
        /// Script-Generated-WaveSource Requires to Call ManualAwake.
        /// </summary>
        /// <param name="srcWP"> Pre initalized WaveParameter.</param>
        public void _awake(WaveParams srcWP) {
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