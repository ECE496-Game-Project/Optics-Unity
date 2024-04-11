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
    private float SCROLL_SPEED;
    [SerializeField] private const int PANEL_HEIGHT = 20;
    [SerializeField] private const int HIDE_POSITION = 98;
    [SerializeField] private float SCROLL_OFFSET = 70f;
    [SerializeField] private float SCROLL_SPEED_AMPLIFIER = 50f;
    [SerializeField] private float SCROLL_DAMP = 0.1f;
    private bool isPanelExpanded = false;
    private ScrollView Body;
    private Scroller scroller;
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

    void Awake(){
        VisualElement root = doc.rootVisualElement;
        Body = root.Q<ScrollView>("Body");
        addButton = root.Q<Button>(name: "AddButton");

        CloseExpandPanel(root);
        PreRegisterCallback(root);
        setScrollView();
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
        float targetValue = scroller.highValue > 0 ? scroller.highValue + SCROLL_OFFSET : 0;
        DOTween.To(()=>scroller.value, x=> scroller.value = x, targetValue, 0.5f);
    }

    private void setScrollView(){
        Body.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        scroller = Body.verticalScroller;
        scroller.valueChanged += ChangeSpeed;
        Body.RegisterCallback<WheelEvent>(ScrollCallback);
    }
    public void ScrollCallback(WheelEvent evt){
        Body.UnregisterCallback<WheelEvent>(ScrollCallback);
        SCROLL_SPEED += evt.delta.y * SCROLL_SPEED_AMPLIFIER;
        evt.StopPropagation();
        Body.RegisterCallback<WheelEvent>(ScrollCallback);
    }
    public void ChangeSpeed(float num){
        scroller.valueChanged -= ChangeSpeed;
        scroller.value += SCROLL_SPEED;
        SCROLL_SPEED -= SCROLL_SPEED * SCROLL_DAMP;
        scroller.valueChanged += ChangeSpeed;
    }
}
