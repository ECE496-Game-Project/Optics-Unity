using UnityEngine;
using System;
using CommonUtils;
using WaveUtils;
using Interfaces;
using ParameterTransfer;
using Panel;
using SelectItems;

namespace GO_Wave {
    public class Wave : MonoBehaviour/*, I_ParameterPanel*/ {
        //public virtual string ParamTransferName { get { return "ChildWave";} }
        public static GameObject NewLineWave(WaveParam roWP, LayerMask interactMask, float sampleRes) {
            GameObject new_GO = new GameObject(
                "LineWave",
                /*typeof(BoxCollider),*/
                typeof(Wave),
                typeof(LineWaveDisplay),
                typeof(LineWaveLogic)/*,
                typeof(SelectableWave)*/
            );
            new_GO.GetComponent<Wave>().Init(roWP);
            new_GO.GetComponent<LineWaveDisplay>().Init(sampleRes);
            new_GO.GetComponent<LineWaveLogic>().Init(interactMask);
            return new_GO;
        }

        // Pointer to Display and Logic handled specially due to sequence of Component
        // we add in the NewLineWave
        private I_WaveDisplay m_waveDisplay;
        public I_WaveDisplay WaveDisplay {
            get { if(m_waveDisplay==null) m_waveDisplay = GetComponent<I_WaveDisplay>(); return m_waveDisplay; }
        }
        private I_WaveLogic m_waveLogic;
        public I_WaveLogic WaveLogic {
            get { if (m_waveLogic == null) m_waveLogic = GetComponent<I_WaveLogic>(); return m_waveLogic; }
        }

        private WaveParam m_params;
        public WaveParam Params {
            get { return m_params; }
            set {
                if (m_params != null)
                    DebugLogger.Error(this.name, "Re-Initalize WaveParameter! Break.");
                m_params = value;
            }
        }

        #region GLOBAL METHODS
        public virtual void ParameterChangeTrigger() {
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
        //protected void RegisterDirCallback() {
        //    m_params.UHat = this.transform.right;
        //    m_params.VHat = this.transform.up;
        //    m_params.KHat = this.transform.forward;
        //    m_params.Origin = this.transform.position;
        //}

        //public void RegisterParametersCallback(ParameterInfoList ParameterInfos) {
        //    var NameTuple = (ParameterInfo<string>)ParameterInfos.SymbolQuickAccess["Name"];
        //    var EoxTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["UdirAmp"];
        //    var EoyTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["VdirAmp"];
        //    var thetaTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["Theta"];
        //    var TTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["T"];
        //    var muTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["mu"];
        //    var wTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["w"];
        //    var lambdaTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["lambda"];
        //    var fTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["f"];
        //    var kTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["k"];
        //    var phiTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["Phi"];
        //    var nTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["N"];

        //    NameTuple.Getter = () => { return this.name; };
        //    EoxTuple.Getter = () => { return m_params.Eox; };
        //    EoyTuple.Getter = () => { return m_params.Eoy; };
        //    thetaTuple.Getter = () => { return m_params.theta; };
            
        //    TTuple.Getter = () => { return m_params.T; };
        //    muTuple.Getter = () => { return m_params.mu; };
        //    wTuple.Getter = () => { return m_params.w; };
            
        //    lambdaTuple.Getter = () => { return m_params.lambda; };
        //    fTuple.Getter = () => { return m_params.f; };
        //    kTuple.Getter = () => { return m_params.k; };
            
        //    phiTuple.Getter = () => { return m_params.phi; };
        //    nTuple.Getter = () => { return m_params.n; };


        //    NameTuple.Setter = (evt) => { /*this.name = evt.newValue;*/ };
        //    EoxTuple.Setter = (evt) => { m_params.Eox = evt.newValue; ParameterChangeTrigger(); };
        //    EoyTuple.Setter = (evt) => { m_params.Eoy = evt.newValue; ParameterChangeTrigger(); };
        //    thetaTuple.Setter = (evt) => { m_params.theta = evt.newValue; ParameterChangeTrigger(); };
            
        //    TTuple.Setter = (evt) => { DebugLogger.Warning(this.name, "T Setter should not be use, something wrong."); };
        //    muTuple.Setter = (evt) => {
        //        m_params.mu = evt.newValue;
        //        WaveAlgorithm.changeMu(m_params);
        //        ParamPanelManager.Instance.CallGetter();
        //        ParameterChangeTrigger();
        //    };
        //    wTuple.Setter = (evt) => {DebugLogger.Warning(this.name, "w Setter should not be use, something wrong.");};
            
        //    lambdaTuple.Setter = (evt) => {
        //        m_params.lambda = evt.newValue; 
        //        WaveAlgorithm.changeLambda(m_params); 
        //        ParamPanelManager.Instance.CallGetter(); 
        //        ParameterChangeTrigger(); 
        //    };
        //    fTuple.Setter = (evt) => { DebugLogger.Warning(this.name, "f Setter should not be use, something wrong."); };
        //    kTuple.Setter = (evt) => { DebugLogger.Warning(this.name, "k Setter should not be use, something wrong."); };
            
        //    nTuple.Setter = (evt) => { m_params.n = evt.newValue; WaveAlgorithm.changeN(m_params); ParameterChangeTrigger(); };
        //    phiTuple.Setter = (evt) => { m_params.phi = evt.newValue; ParameterChangeTrigger(); };
        //}

        /// <summary>
        /// Script-Generated-WaveSource Requires to Call ManualAwake.
        /// </summary>
        /// <param name="srcWP"> Pre initalized WaveParameter.</param>
        public void Init(WaveParam srcWP) {
            //WaveAlgorithm.changeT(srcWP);
            //if (srcWP.Type == WAVETYPE.INVALID)
            //    DebugLogger.Error(this.name, "SourceWave Parameter Type Invalid! Stop Executing.");
            //WaveDisplay = GetComponent<I_WaveDisplay>();
            //if (WaveDisplay == null)
            //    DebugLogger.Error(this.name, "GameObject Does not contain WaveDisplay! Stop Executing.");
            //WaveLogic = GetComponent<I_WaveLogic>();
            //if (WaveLogic == null)
            //    DebugLogger.Error(this.name, "GameObject Does not contain WaveInteract! Stop Executing.");

            /*init ActiveWaveParams*/
            m_params = srcWP;
            //RegisterDirCallback();
        }

        // for child-wave Trigger have to wait for Logic & Render initalized, thus in Start
        public void Start() {
            ParameterChangeTrigger();
        }
    }
}