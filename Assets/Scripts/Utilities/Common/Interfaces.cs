using UnityEngine;

namespace Interfaces {
    public interface I_WaveDisplay {
        public void CleanDisplay();
        public void RefreshDisplay();
        public void UpdateDisplay();
        public void SyncRootParam(I_WaveDisplay srcWD);
    }
    public interface I_WaveInteract {
        public void CleanInteract();
        public void Interact();
        public void SyncRootParam(I_WaveInteract srcWI);
    }
}