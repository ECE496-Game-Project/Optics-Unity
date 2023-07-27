using UnityEngine;

namespace ns_Interface {
    public interface I_WaveDisplay {
        public void RefreshDisplay();
        public void UpdateDisplay();
        public void Prepare(I_WaveDisplay srcWD);
    }
    public interface I_WaveInteract {
        public void DestructInteract();
        public void NonDestructInteract();
        public void Prepare(I_WaveInteract srcWI);
    }
    public interface I_WaveInput {
        public void Rotate(Vector3 rot);
        public void Move(Vector3 dest);
    }
}