using Interfaces;
using ParameterTransfer;
using WaveUtils;

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
            EoxTuple.Getter = () => { return WaveAlgorithm.FloatRounding2dec(m_params.Eox); };
            EoyTuple.Getter = () => { return WaveAlgorithm.FloatRounding2dec(m_params.Eoy); };
            thetaTuple.Getter = () => { return WaveAlgorithm.FloatRounding2dec(m_params.Theta); };

            
            TTuple.Getter = () => { return m_params.T * WaveAlgorithm.fsPerUnitySecond;  };
            // we multiply 1000 because right now the unit is PHz, but we want to convert it to THz
            muTuple.Getter = () => { return m_params.Mu / WaveAlgorithm.fsPerUnitySecond * 1000f; };
            wTuple.Getter = () => { return m_params.W / WaveAlgorithm.fsPerUnitySecond; };

            lambdaTuple.Getter = () => { return m_params.Lambda * WaveAlgorithm.nmPerUnit; };
            fTuple.Getter = () => { return m_params.F / WaveAlgorithm.nmPerUnit; };
            kTuple.Getter = () => { return m_params.K / WaveAlgorithm.nmPerUnit; };

            phiTuple.Getter = () => { return WaveAlgorithm.FloatRounding2dec(m_params.Phi); };
            nTuple.Getter = () => { return WaveAlgorithm.FloatRounding2dec(m_params.N); };
        }
    }
}