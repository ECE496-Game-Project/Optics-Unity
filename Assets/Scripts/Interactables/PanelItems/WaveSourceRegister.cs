using Interfaces;
using ParameterTransfer;

namespace GO_Wave {
    public partial class WaveSource : I_ParameterPanel {
        public string CorrespondingUIInfoName { get { return "WaveSource";} }
        
        public void ParameterChangeTrigger() {
            Emit();
        }

        public void RegisterParametersCallback(ParameterInfoList ParameterInfos) {
            var NameTuple = (ParameterInfo<string>)ParameterInfos.SymbolQuickAccess["Name"];
            var EoxTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["UdirAmp"];
            var EoyTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["VdirAmp"];
            var thetaTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["Theta"];
            var lambdaTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["lambda"];
            var nTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["N"];

            NameTuple.Getter = () => { return this.name; };
            EoxTuple.Getter = () => { return m_param.Eox; };
            EoyTuple.Getter = () => { return m_param.Eoy; };
            thetaTuple.Getter = () => { return m_param.theta; };
            lambdaTuple.Getter = () => { return m_param.lambda; };
            nTuple.Getter = () => { return m_param.n; };


            NameTuple.Setter = (evt) => { this.name = evt.newValue; };
            EoxTuple.Setter = (evt) => { m_param.Eox = evt.newValue; ParameterChangeTrigger(); };
            EoyTuple.Setter = (evt) => { m_param.Eoy = evt.newValue; ParameterChangeTrigger(); };
            thetaTuple.Setter = (evt) => { m_param.theta = evt.newValue; ParameterChangeTrigger(); };
            lambdaTuple.Setter = (evt) => { if (evt.newValue == 0) return; m_param.lambda = evt.newValue; ParameterChangeTrigger(); };
            nTuple.Setter = (evt) => { if (evt.newValue > 5 || evt.newValue < 1) return; m_param.n = evt.newValue; ParameterChangeTrigger(); };
        }
    }
}