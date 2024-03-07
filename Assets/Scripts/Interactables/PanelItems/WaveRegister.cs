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
            thetaTuple.Getter = () => { return m_params.Theta; };

            // we divide 1000 because right now the unit is PHz, but we want to convert it to THz
            TTuple.Getter = () => { return m_params.T * TempSingletonManager.Instance.m_scaleManager.fsPerUnitySecond / 1000f; };
            muTuple.Getter = () => { return m_params.Mu / TempSingletonManager.Instance.m_scaleManager.nmPerUnit; };
            wTuple.Getter = () => { return m_params.W / TempSingletonManager.Instance.m_scaleManager.nmPerUnit; };

            lambdaTuple.Getter = () => { return m_params.Lambda * TempSingletonManager.Instance.m_scaleManager.nmPerUnit; };
            fTuple.Getter = () => { return m_params.F / TempSingletonManager.Instance.m_scaleManager.nmPerUnit; };
            kTuple.Getter = () => { return m_params.K / TempSingletonManager.Instance.m_scaleManager.nmPerUnit; };

            phiTuple.Getter = () => { return m_params.Phi; };
            nTuple.Getter = () => { return m_params.N; };
        }
    }
}