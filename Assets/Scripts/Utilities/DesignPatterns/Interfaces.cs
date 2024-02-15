using ParameterTransfer;
using UnityEngine;

namespace Interfaces {
    public interface I_WaveDisplay {
        public void CleanDisplay();
        public void RefreshDisplay();
        public void Init(float sampleResolution);
    }
    public interface I_WaveLogic {
        public void CleanInteract();
        public void Interact();
        public void Init(LayerMask interactMask);
    }
    public interface I_ParameterPanel {
        public string CorrespondingUIInfoName { get; }
        public void RegisterParametersCallback(ParameterInfoList ParameterInfos);
        public void ParameterChangeTrigger();
    }
}