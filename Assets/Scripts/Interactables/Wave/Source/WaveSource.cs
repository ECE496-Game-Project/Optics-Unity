using UnityEngine;
using System;
using CommonUtils;
using WaveUtils;
using Interfaces;
using ParameterTransfer;
using Panel;

namespace GO_Wave {
    public class WaveSource : MonoBehaviour, I_ParameterPanel {
        public virtual string ParamTransferName { get { return "ChildWave";} }

        #region GLOBAL VARIABLES
        public I_WaveRender WaveDisplay;
        public I_WaveLogic WaveLogic;
        #endregion

        #region PRIVATE VARIABLES
        protected WaveParams m_params;
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

        public virtual void ParameterChangeTrigger() {
            // Refresh EffectDistance from ReadOnly Value
            WaveLogic.CleanInteract();
            WaveLogic.Interact();
            WaveDisplay.RefreshDisplay();
        }

        public void WaveClean() {
            WaveLogic.CleanInteract();
            WaveDisplay.CleanDisplay();
        }
        #endregion

        // store Orientation of WaveSource into WaveParam
        protected void RegisterDirCallback() {
            m_params.UHat = this.transform.right;
            m_params.VHat = this.transform.up;
            m_params.KHat = this.transform.forward;
            m_params.Origin = this.transform.position;
        }

        public void RegisterParametersCallback(ParameterInfoList ParameterInfos) {
            var NameTuple = (ParameterInfo<string>)ParameterInfos.SymbolQuickAccess["Name"];
            var EoxTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["UdirAmp"];
            var EoyTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["VdirAmp"];
            var thetaTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["Theta"];
            var TTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["T"];
            var muTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["mu"];
            var wTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["w"];
            var lambdaTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["lambda"];
            var fTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["f"];
            var kTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["k"];
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


            NameTuple.Setter = (evt) => { /*this.name = evt.newValue;*/ };
            EoxTuple.Setter = (evt) => { m_params.Eox = evt.newValue; ParameterChangeTrigger(); };
            EoyTuple.Setter = (evt) => { m_params.Eoy = evt.newValue; ParameterChangeTrigger(); };
            thetaTuple.Setter = (evt) => { m_params.theta = evt.newValue; ParameterChangeTrigger(); };
            
            TTuple.Setter = (evt) => { DebugLogger.Warning(this.name, "T Setter should not be use, something wrong."); };
            muTuple.Setter = (evt) => {
                m_params.mu = evt.newValue;
                WaveAlgorithm.changeMu(m_params);
                ParamPanelManager.Instance.CallGetter();
                ParameterChangeTrigger();
            };
            wTuple.Setter = (evt) => {DebugLogger.Warning(this.name, "w Setter should not be use, something wrong.");};
            
            lambdaTuple.Setter = (evt) => {
                m_params.lambda = evt.newValue; 
                WaveAlgorithm.changeLambda(m_params); 
                ParamPanelManager.Instance.CallGetter(); 
                ParameterChangeTrigger(); 
            };
            fTuple.Setter = (evt) => { DebugLogger.Warning(this.name, "f Setter should not be use, something wrong."); };
            kTuple.Setter = (evt) => { DebugLogger.Warning(this.name, "k Setter should not be use, something wrong."); };
            
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
            WaveLogic = GetComponent<I_WaveLogic>();
            if (WaveLogic == null)
                DebugLogger.Error(this.name, "GameObject Does not contain WaveInteract! Stop Executing.");

            /*init ActiveWaveParams*/
            m_params = srcWP;
            RegisterDirCallback();
        }

        // for child-wave Trigger have to wait for Logic & Render initalized, thus in Start
        public void Start() {
            ParameterChangeTrigger();
        }
    }
}