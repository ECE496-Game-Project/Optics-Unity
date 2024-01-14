using UnityEngine;
using CommonUtils;
using WaveUtils;
using Profiles;
using Interfaces;
using ParameterTransfer;

namespace GO_Wave {
    public class RootWaveSource : WaveSource {
        #region INSPECTOR SETTINGS
        [SerializeField] private SO_WaveParams _profile;
        #endregion

        /// <summary>   
        /// Only Called if Profile is Set In Inspector
        /// </summary>
        private void Awake() {
            if(_profile == null)
                DebugLogger.Error(this.name, "RootWave Does not contain WaveProfile! Stop Executing.");

            _awake(new WaveParams(_profile.Parameters));
        }

        public override void RegisterParametersCallback(ParameterInfoList ParameterInfos) {
            var NameTuple = (ParameterInfo<string>)ParameterInfos.SymbolQuickAccess["WaveName"];
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

            NameTuple.Getter    = () => { return this.name; };
            EoxTuple.Getter     = () => { return m_params.Eox; };
            EoyTuple.Getter     = () => { return m_params.Eoy; };
            thetaTuple.Getter   = () => { return m_params.theta; };
            TTuple.Getter       = () => { return m_params.T; };
            muTuple.Getter      = () => { return m_params.mu; };
            wTuple.Getter       = () => { return m_params.w; };
            lambdaTuple.Getter  = () => { return m_params.lambda; };
            fTuple.Getter       = () => { return m_params.f; };
            kTuple.Getter       = () => { return m_params.k; };
            phiTuple.Getter     = () => { return m_params.phi; };
            nTuple.Getter       = () => { return m_params.n; };

            NameTuple.Setter    = (evt) => { this.name = evt.newValue; };
            EoxTuple.Setter     = (evt) => { m_params.Eox = evt.newValue; ParameterChangeTrigger(); };
            EoyTuple.Setter     = (evt) => { m_params.Eoy = evt.newValue; ParameterChangeTrigger(); };
            thetaTuple.Setter   = (evt) => { m_params.theta = evt.newValue; ParameterChangeTrigger(); };
            TTuple.Setter       = (evt) => { m_params.T = evt.newValue; WaveAlgorithm.changeT(m_params); ParameterChangeTrigger(); };
            wTuple.Setter       = (evt) => { m_params.w = evt.newValue; WaveAlgorithm.changeW(m_params); ParameterChangeTrigger(); };
            lambdaTuple.Setter  = (evt) => { m_params.lambda = evt.newValue; WaveAlgorithm.changeLambda(m_params); ParameterChangeTrigger(); };
            kTuple.Setter       = (evt) => { m_params.k = evt.newValue; WaveAlgorithm.changeK(m_params); ParameterChangeTrigger(); };
            nTuple.Setter       = (evt) => { m_params.n = evt.newValue; WaveAlgorithm.changeN(m_params); ParameterChangeTrigger(); };
            phiTuple.Setter     = (evt) => { m_params.phi = evt.newValue; ParameterChangeTrigger(); };
        }
    }
}