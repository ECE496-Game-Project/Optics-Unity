using GO_Device;

namespace SelectItems {
    public class SelectableTrack : Selectable {
        public override void OnMouseHover() {
            OutlineManager.Instance.Highlight(this.gameObject);
        }

        public override void OnMouseSelect() {
            ObjectViewPanel.Instance.SelectTrackView(this.GetComponent<Track>());
            
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
