using UnityEngine;
using Panel;

namespace SelectItems {
    public class SelectableDevice : Selectable {
        public override void OnMouseHover() {
            OutlineManager.Instance.Highlight(this.gameObject);
            //Debug.Log(gameObject.name + " is hovered");
        }

        public override void OnMouseSelect() {
            OutlineManager.Instance.Highlight(this.gameObject);
            ParamPanelManager.Instance.SelectParamView(this.gameObject);

        }

        public override void OnMouseUnhover() {
            OutlineManager.Instance.UnHighlight(this.gameObject);
            //Debug.Log(gameObject.name + " is Unhover");
        }

        public override void OnMouseUnselect() {
            OutlineManager.Instance.UnHighlight(this.gameObject);
            //Debug.Log(gameObject.name + " is Unselect");
        }

        private void Awake() {
            if(Name == null) Name = "PolarizedDevice";
        }
    }
}
