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

            NameTuple.Setter    = (evt) => { this.name = evt.newValue; };
            EoxTuple.Setter     = (evt) => { m_params.Eox = evt.newValue; ParamChangeTrigger(); };
            EoyTuple.Setter     = (evt) => { m_params.Eoy = evt.newValue; ParamChangeTrigger(); };
            thetaTuple.Setter   = (evt) => { m_params.theta = evt.newValue; ParamChangeTrigger(); };
            TTuple.Setter       = (evt) => { m_params.T = evt.newValue; WaveAlgorithm.changeT(m_params); ParamChangeTrigger(); };
            wTuple.Setter       = (evt) => { m_params.w = evt.newValue; WaveAlgorithm.changeW(m_params); ParamChangeTrigger(); };
            lambdaTuple.Setter  = (evt) => { m_params.lambda = evt.newValue; WaveAlgorithm.changeLambda(m_params); ParamChangeTrigger(); };
            kTuple.Setter       = (evt) => { m_params.k = evt.newValue; WaveAlgorithm.changeK(m_params); ParamChangeTrigger(); };
            nTuple.Setter       = (evt) => { m_params.n = evt.newValue; WaveAlgorithm.changeN(m_params); ParamChangeTrigger(); };
            phiTuple.Setter     = (evt) => { m_params.phi = evt.newValue; ParamChangeTrigger(); };
        }

        /// <summary>
        /// Need Manual Reset Effective Distance since Distance Modified during Interaction.
        /// </summary>
        public override void ParamChangeTrigger() {
            EffectDistance = _profile.Parameters.RODistance;
            base.ParamChangeTrigger();
        }
    }
}