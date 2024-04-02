using System;
using UnityEngine;
using UnityEngine.UIElements;
using GO_Device;
using CommonUtils;
using DG.Tweening;

public class ObjectViewPanel : MonoSingleton<ObjectViewPanel>
{
    public UIDocument doc;
    public VisualTreeAsset TrackSliderAsset;

    private const int PANEL_HEIGHT = 20;
    private const int HIDE_POSITION = 98;
    private bool isPanelExpanded = false;
    private ScrollView Body;
    private Button addButton;
    private Action myButtonClickedAction;

    public void PreRegisterCallback(VisualElement root) {
        Button expButton = root.Q<Button>(name: "ExpandButton");
        expButton.clicked += () => {
            if(isPanelExpanded) CloseExpandPanel(root);
            else OpenExpandPanel(root);
        };
    }
    /// <summary>
    /// initalization of dynamic slider
    /// </summary>
    /// <param name="info"></param>
    /// <param name="track"></param>
    /// <param name="prec">if set to -1 then calculated from track </param>
    public void initSlider(TrackSlideInfo info, Track track, float prec) {
        VisualElement Container = TrackSliderAsset.Instantiate();
        Body.Add(Container);

        Slider precSlider = Container.Q<Slider>();
        precSlider.label = info.device.gameObject.name;
        precSlider.value = (prec < 0)? track.GetPrec(info) : prec;
        precSlider.RegisterValueChangedCallback(evt => {
            info.UIPrecChangeCallTrack.Invoke(evt.newValue);
        });
        info.TrackPrecChangeCallUI.RemoveAllListeners();
        info.TrackPrecChangeCallUI.AddListener((float value) => {
            precSlider.value = value;
        });

        Button delete = Container.Q<Button>();
        delete.clicked += () => {
            track.RemoveDevice(info);
            Body.Remove(Container);
        };
    }

    public void CleanTrackView() {
        Body.Clear();
    }

    public void SelectTrackView(Track track) {
        if (!isPanelExpanded) OpenExpandPanel(doc.rootVisualElement);
        CleanTrackView();

        foreach (TrackSlideInfo info in track.DevicesOnTrack) {
            initSlider(info, track, -1);
        }

        // Ensure we don't subscribe multiple times if Start is called again.
        if (myButtonClickedAction != null) {
            addButton.clicked -= myButtonClickedAction;
        }

        myButtonClickedAction = () => {
            var info = track.AddDevice();
            initSlider(info, track, 1);
            ScrollToBottom();
        };

        addButton.clicked += myButtonClickedAction;
    }

    void Awake()
    {
        VisualElement root = doc.rootVisualElement;
        Body = root.Q<ScrollView>("Body");
        addButton = root.Q<Button>(name: "AddButton");

        CloseExpandPanel(root);
        PreRegisterCallback(root);
    }

    public void CloseExpandPanel(VisualElement root){
        VisualElement expPanel = root.Q<VisualElement>(name: "ExpandPanel");
        ScrollView expBody = root.Q<ScrollView>(name: "Body");
        Button expButton = root.Q<Button>(name: "ExpandButton");
        expPanel.style.height = PANEL_HEIGHT;
        expPanel.style.maxHeight = PANEL_HEIGHT;
        Length height = new Length(HIDE_POSITION, LengthUnit.Percent);
        expPanel.style.top = new StyleLength(height);
        expBody.style.display = DisplayStyle.None;
        expButton.text = "\u2191";
        isPanelExpanded = false;
    }
    public void OpenExpandPanel(VisualElement root){
        VisualElement expPanel = root.Q<VisualElement>(name: "ExpandPanel");
        ScrollView expBody = root.Q<ScrollView>(name: "Body");
        Button expButton = root.Q<Button>(name: "ExpandButton");
        Length height = new Length(PANEL_HEIGHT, LengthUnit.Percent);
        expPanel.style.height = new StyleLength(height);
        expPanel.style.maxHeight = new StyleLength(height);
        height = new Length(100 - PANEL_HEIGHT, LengthUnit.Percent);
        expPanel.style.top = new StyleLength(height);
        expBody.style.display = DisplayStyle.Flex;
        expButton.text = "\u2193";
        isPanelExpanded = true;
    }

    private void ScrollToBottom(){
        Scroller scroller = Body.verticalScroller;
        float targetValue = scroller.highValue > 0 ? scroller.highValue : 0;
        DOTween.To(()=>scroller.value, x=> scroller.value = x, targetValue, 0.5f);
    }
}
