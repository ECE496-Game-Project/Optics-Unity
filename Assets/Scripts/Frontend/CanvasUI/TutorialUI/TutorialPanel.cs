using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

using Ink.Runtime;
using DG.Tweening;
using CommonUtils;

public class TutorialPanel : MonoSingleton<TutorialPanel>
{
    [Header("Params")]
    [SerializeField] private float TYPE_SPEED = 0.04f;
    private float SCROLL_SPEED;
    [SerializeField] private float SCROLL_SPEED_AMPLIFIER = 50f;
    [SerializeField] private float SCROLL_DAMP = 0.1f;
    [SerializeField] private float SCROLL_OFFSET = 100f;
    [SerializeField] private float SPACER_HEIGHT = 200f; 
    [SerializeField] private float MIN_WIDTH = 500f; 
    [SerializeField] private float EXIT_LAG_TIME = 0.5f;
    [SerializeField] private int PANEL_WIDTH = 30;
    [SerializeField] private float HIDE_POSITION = 98.5f;
    
    [Header("UI")]
    public UIDocument doc;

    [Header("Tutorial Content")]
    [SerializeField] private TextAsset globalJSON;
    [SerializeField] private TextAsset defaultInkJSON;

    private VisualElement root;
    private VisualElement expPanel;
    private VisualElement expBody;
    private bool isMouseOverElement = false;


    private Label title;
    private ScrollView content;
    private Scroller scroller;
    private Button expand;
    private Button pause;
    private Button system;
    private VisualElement spacer;
    private VisualTreeAsset realChoice;
    private VisualTreeAsset fakeChoice;
    private VisualTreeAsset textArea;
    private VisualTreeAsset imgArea;

    private bool isPanelExpanded = true;
    private bool isPaused = false;

    private bool tutIsPlaying = false;
    private bool canGoToNextLine = false;
    private Story currStory;
    private Coroutine displayLine;

    private const string SPEAKER_TAG = "speaker";
    private const string TITLE_TAG = "title";
    private const string PORTRAIT_TAG = "portrait";
    private const string IMG_TAG = "image";

    private string displaySpeakerName = "";
    private TutorialVariables dialogueVariables;

    private void Awake()
    {
        root = doc.rootVisualElement;
        
        expPanel = root.Q<VisualElement>(name: "ExpandPanel");
        expBody = root.Q<VisualElement>(name: "Body");
        
        expand = root.Q<Button>(name: "ExpandButton");
        pause = root.Q<Button>(name: "PauseButton");
        system = root.Q<Button>(name: "SystemButton");

        title = root.Q<Label>(name: "title");

        setScrollView();

        realChoice = Resources.Load<VisualTreeAsset>("Art/Frontend/Documents/TutorialPanel/RealChoice");
        fakeChoice = Resources.Load<VisualTreeAsset>("Art/Frontend/Documents/TutorialPanel/FakeChoice");
        textArea = Resources.Load<VisualTreeAsset>("Art/Frontend/Documents/TutorialPanel/TextArea");
        imgArea = Resources.Load<VisualTreeAsset>("Art/Frontend/Documents/TutorialPanel/ImgArea");

        dialogueVariables = new TutorialVariables(globalJSON);
    }

    private void Start()
    {   
        OpenExpandPanel();
        PreRegisterCallback();
        BeginTutorial(defaultInkJSON);

         // Assuming you have already obtained a reference to your specific VisualElement
        expBody.RegisterCallback<MouseEnterEvent>(evt => MouseEntered(evt));
        expBody.RegisterCallback<MouseLeaveEvent>(evt => MouseLeft(evt));
    }

    private void Update(){
        if (!tutIsPlaying) return;

        if (canGoToNextLine && IsUserInput() 
        && currStory.currentChoices.Count == 0){
            ContinueStory();
        }
    }

    #region Panel
    public void CloseExpandPanel(){
        expPanel.style.width = PANEL_WIDTH;
        Length width = new Length(HIDE_POSITION, LengthUnit.Percent);
        expPanel.style.left = new StyleLength(width);
        expBody.style.display = DisplayStyle.None;

        expand.text = "\u2190";
        isPanelExpanded = false;
    }

    public void OpenExpandPanel(){
        Length width = new Length(PANEL_WIDTH, LengthUnit.Percent);
        expPanel.style.width = new StyleLength(width);
        width = new Length(100 - PANEL_WIDTH, LengthUnit.Percent);
        expPanel.style.left = new StyleLength(width);
        expBody.style.display = DisplayStyle.Flex;

        expand.text = "\u2192";
        isPanelExpanded = true;
    }

    public void PreRegisterCallback() {
        expand.clicked += () => {
            if(isPanelExpanded) CloseExpandPanel();
            else OpenExpandPanel();
        };

        pause.clicked += () => {
            if (!isPaused) {
                pause.style.backgroundImage = Resources.Load<Texture2D>("Art/Images/Icons/play");
                WaveTime.PauseTime();
            }
            else {
                pause.style.backgroundImage = Resources.Load<Texture2D>("Art/Images/Icons/pause");
                WaveTime.ResumeTime();
            }
            isPaused = !isPaused;
        };

        system.clicked += () => {
            this.BeginTutorial(defaultInkJSON);
        };
    }
    #endregion

    #region Tutorial

    public void BeginTutorial(TextAsset inkJSON){
        currStory = new Story(inkJSON.text);
        
        dialogueVariables.StartListening(currStory);
        // TODO: Load variables
        
        tutIsPlaying = true;
        title.text = "???";
        displaySpeakerName = "???";

        content.contentContainer.Clear();
        DisplaySpacer();

        OpenExpandPanel();
        ContinueStory();
    }

    private void ContinueStory(){
        if(!currStory.canContinue){
            StartCoroutine(ExitTutorial());
            return;
        }

        if(displayLine != null) StopCoroutine(displayLine); 
        displayLine = StartCoroutine(DisplayLine(currStory.Continue()));
        
        HandleTags(currStory.currentTags);
        MoveSpacerToEnd();
        ScrollToBottom();
    }

    private IEnumerator ExitTutorial(){
        yield return new WaitForSeconds(EXIT_LAG_TIME);
        
        dialogueVariables.StopListening(currStory);
        dialogueVariables.SaveVariables();
        
        tutIsPlaying = false;
        displaySpeakerName = "";

        CloseExpandPanel();
    }

    public void OnApplicationQuit() {
        dialogueVariables.SaveVariables();
    }

    #region Render
    private IEnumerator DisplayLine(string line){
        VisualElement textLine = textArea.Instantiate();
        Label label = textLine.Q<Label>();
        label.text = displaySpeakerName + "-";
        content.Add(textLine);

        canGoToNextLine = false;

        foreach (char letter in line.ToCharArray()){
            if (IsUserInput()) {
                label.text = displaySpeakerName + "-" + line;
                break;
            }

            label.text += letter;
            yield return new WaitForSeconds(TYPE_SPEED);
        }

        DisplayChoices();

        canGoToNextLine = true;
    }

    private void DisplayChoices(){
        List<Choice> currChoices = currStory.currentChoices;
        
        // 1. Fake Choice
        if(currChoices.Count == 1){
            VisualElement choice = fakeChoice.Instantiate();
            Button button = choice.Q<Button>();
            button.text = currChoices[0].text + " " + '\u25B6';
            button.clicked += () => {
                MakeChoice(currChoices[0], choice);
            };
            content.Add(choice);
            return;
        }

        // 2. Real Choice
        List<VisualElement> realChoices = new List<VisualElement>();
        foreach(Choice choice in currChoices) {   
            VisualElement choiceElement = realChoice.Instantiate();
            realChoices.Add(choiceElement);
            content.Add(choiceElement);
        }

        int index = 0;
        foreach(Choice choice in currChoices){
            Button button = realChoices[index].Q<Button>();
            button.text = index + ".-" + choice.text;
            button.clicked += () => {
                MakeChoice(choice, realChoices);
            };
            index++;
        }
    }

    private void DisplayImage(string imgVal){
        VisualElement imgContainer = imgArea.Instantiate();
        VisualElement img = imgContainer.Q<VisualElement>(name:"Image");
        
        Sprite sp = Resources.Load<Sprite>("Art/Images/" + imgVal);
        if(sp == null){
            Debug.LogError("Can't find image: " + imgVal);
            return;
        }

        if(img.style.width.value.value == 0){
            img.style.width = MIN_WIDTH;
            Debug.LogWarning("No valid width!");
        } 
        float aspectRatio = (float)sp.textureRect.height / (float)sp.textureRect.width;
        img.style.height = new StyleLength(img.style.width.value.value * aspectRatio);
        img.style.backgroundImage = new StyleBackground(sp);
        content.Add(imgContainer);
    }

    private void DisplaySpacer(){
        if(spacer != null){
            if(content.Contains(spacer)) content.Remove(spacer);
            spacer = null;
        }
        spacer = new VisualElement();
        spacer.style.height = SPACER_HEIGHT;
        content.Add(spacer);
    }

    #endregion

    #region Logic
    private void MakeChoice(Choice choice, List<VisualElement> choices){
        if (!canGoToNextLine) return;

        foreach(VisualElement choiceEl in choices){
            content.Remove(choiceEl);
        }
        VisualElement textLine = textArea.Instantiate();
        textLine.Q<Label>().text = "You-\"" + choice.text + "\"";
        content.Add(textLine);
        currStory.ChooseChoiceIndex(choice.index);
        ContinueStory();
    }

    private void MakeChoice(Choice choice, VisualElement choiceEl){
        if (!canGoToNextLine) return;
        content.Remove(choiceEl);
        currStory.ChooseChoiceIndex(choice.index);
        ContinueStory();
    }

    private void MoveSpacerToEnd(){
        float contentHeight = content.contentContainer.layout.height;
        float bottomOffset = Mathf.Max(0, contentHeight + spacer.layout.height/2f);
        spacer.style.top = bottomOffset;
    }

    private void ScrollToBottom(){
        float targetValue = scroller.highValue > 0 ? scroller.highValue + SCROLL_OFFSET : 0;
        DOTween.To(()=>scroller.value, x=> scroller.value = x, targetValue, EXIT_LAG_TIME);
    }

    private void setScrollView(){
        content = root.Q<ScrollView>(name: "content");
        content.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        scroller = content.verticalScroller;
        scroller.valueChanged += ChangeSpeed;
        content.RegisterCallback<WheelEvent>(ScrollCallback);
    }

    public void ScrollCallback(WheelEvent evt){
        content.UnregisterCallback<WheelEvent>(ScrollCallback);
        SCROLL_SPEED += evt.delta.y * SCROLL_SPEED_AMPLIFIER;
        evt.StopPropagation();
        content.RegisterCallback<WheelEvent>(ScrollCallback);
    }
 
    public void ChangeSpeed(float num){
        scroller.valueChanged -= ChangeSpeed;
        scroller.value += SCROLL_SPEED;
        SCROLL_SPEED -= SCROLL_SPEED * SCROLL_DAMP;
        scroller.valueChanged += ChangeSpeed;
    }

    private void HandleTags(List<string> tags){
        foreach (string tag in tags) {
            // parse the tag
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2) {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();
            
            // handle the tag
            switch (tagKey) {
                case SPEAKER_TAG:
                    displaySpeakerName = tagValue;
                    break;
                case TITLE_TAG:
                    title.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    break;
                case IMG_TAG:
                    DisplayImage(tagValue);
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }
    #endregion

    public Ink.Runtime.Object GetVariableState(string varName){
        Ink.Runtime.Object varValue = null;
        dialogueVariables.variables.TryGetValue(varName, out varValue);
        if (varValue == null) {
            Debug.LogWarning("Ink Variable was found to be null: " + varName);
        }
        return varValue;
    }

    private void MouseEntered(MouseEnterEvent evt)
    {
        isMouseOverElement = true;
    }

    private void MouseLeft(MouseLeaveEvent evt)
    {
        isMouseOverElement = false;
    }

    private bool IsUserInput(){

        return TutorialController.Instance.isInput && isMouseOverElement;
    }

    #endregion
}
