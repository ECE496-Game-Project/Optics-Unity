using UnityEngine;

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
}