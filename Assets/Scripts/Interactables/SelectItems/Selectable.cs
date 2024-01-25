using UnityEngine;

namespace SelectItems {
    public abstract class Selectable : MonoBehaviour {
        public string Name;
        public abstract void OnMouseSelect();
        public abstract void OnMouseUnselect();
        public abstract void OnMouseHover();
        public abstract void OnMouseUnhover();
    }
}