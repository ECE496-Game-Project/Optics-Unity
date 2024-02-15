using Interfaces;
using ParameterTransfer;

namespace GO_Wave {
    public partial class Wave : I_ParameterPanel {
        public string CorrespondingUIInfoName { get { return "Wave";} }
        
        public void ParameterChangeTrigger() {
            WaveVisualize();
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
        }
    }
}