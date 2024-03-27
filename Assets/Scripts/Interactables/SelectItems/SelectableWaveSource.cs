using UnityEngine;
using Panel;

namespace SelectItems {
    public class SelectableWaveSource : Selectable {
        public override void OnMouseHover() {
            OutlineManager.Instance.Highlight(this.gameObject);
        }

        public override void OnMouseSelect() {
            OutlineManager.Instance.Highlight(this.gameObject);
            ParamPanelManager.Instance.SelectParamView(this.gameObject);

            TutorialPanel.Instance.BeginTutorial(inkJSON);
        }

        public override void OnMouseUnhover() {
            OutlineManager.Instance.UnHighlight(this.gameObject);
        }

        public override void OnMouseUnselect() {
            OutlineManager.Instance.UnHighlight(this.gameObject);
        }
    }
}
