using ParameterTransfer;

namespace Interfaces {
    public interface I_WaveRender {
        public void CleanDisplay();
        public void RefreshDisplay();
        public void UpdateDisplay();
        public void SyncRootParam(I_WaveRender srcWD);
    }
    public interface I_WaveLogic {
        public void CleanInteract();
        public void Interact();
        public void SyncRootParam(I_WaveLogic srcWI);
    }
    public interface I_ParameterTransfer {
        public void RegisterParametersCallback(ParameterInfoList ParameterInfos);
        public void ParameterChangeTrigger();
    }
    public interface ISelectable {
        public void OnMouseSelect();
        public void OnMouseUnselect();
    }
}