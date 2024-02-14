using ParameterTransfer;

namespace Interfaces {
    public interface I_WaveRender {
        public void CleanDisplay();
        public void RefreshDisplay();
        public void UpdateDisplay();
        public void init(I_WaveRender srcWD);
    }
    public interface I_WaveLogic {
        public void CleanInteract();
        public void Interact();
        public void init(I_WaveLogic srcWI);
    }
    public interface I_ParameterPanel {
        public string ParamTransferName { get; }
        public void RegisterParametersCallback(ParameterInfoList ParameterInfos);
        public void ParameterChangeTrigger();
    }
}