using UnityEngine;
using Ink.Runtime;

namespace SelectItems {
    public abstract class Selectable : MonoBehaviour {
        [Header("Tutorial Content")]
        [SerializeField] protected TextAsset inkJSON;
        public abstract void OnMouseSelect();
        public abstract void OnMouseUnselect();
        public abstract void OnMouseHover();
        public abstract void OnMouseUnhover();
    }
}