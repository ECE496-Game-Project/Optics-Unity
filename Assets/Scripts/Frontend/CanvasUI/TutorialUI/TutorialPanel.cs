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
    [SerializeField] private float SCROLL_SPEED = 50f;
    [SerializeField] private float SCROLL_WIDTH = 30f;
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
    private Label title;
    private ScrollView content;
    private Button expButton;
    private Button pause;
    private VisualTreeAsset realChoice;
    private VisualTreeAsset fakeChoice;
    private VisualTreeAsset textArea;

    private bool isPanelExpanded = true;
    private bool isPaused = false;

    private bool tutIsPlaying = false;
    private bool canGoToNextLine = false;
    private Story currStory;
    private Coroutine displayLine;

    private const string SPEAKER_TAG = "speaker";
    private const string TITLE_TAG = "title";

    private string displaySpeakerName = "";

    private void Awake()
    {
        root = doc.rootVisualElement;
        
        expPanel = root.Q<VisualElement>(name: "ExpandPanel");
        expBody = root.Q<VisualElement>(name: "Body");
        
        expButton = root.Q<Button>(name: "ExpandButton");
        pause = root.Q<Button>(name: "PauseButton");

        content = root.Q<ScrollView>(name: "content");
        content.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
        content.verticalScroller.style.width = SCROLL_WIDTH;
        content.mouseWheelScrollSize = SCROLL_SPEED;
        content.elasticity = SCROLL_SPEED;

        title = root.Q<Label>(name: "title");

        realChoice = Resources.Load<VisualTreeAsset>("Art/Frontend/Documents/TutorialPanel/RealChoice");
        fakeChoice = Resources.Load<VisualTreeAsset>("Art/Frontend/Documents/TutorialPanel/FakeChoice");
        textArea = Resources.Load<VisualTreeAsset>("Art/Frontend/Documents/TutorialPanel/TextArea");
    }

    private void Start()
    {   
        OpenExpandPanel();
        PreRegisterCallback();
        BeginTutorial(defaultInkJSON);
    }

    private void Update(){
        if (!tutIsPlaying) return;
        
        if (canGoToNextLine && currStory.currentChoices.Count == 0 && isUserInput()){
            ContinueStory();
        }
    }

    #region Panel
    public void CloseExpandPanel(){
        expPanel.style.width = PANEL_WIDTH;
        Length width = new Length(HIDE_POSITION, LengthUnit.Percent);
        expPanel.style.left = new StyleLength(width);
        expBody.style.display = DisplayStyle.None;

        expButton.text = "\u2190";
        isPanelExpanded = false;
    }

    public void OpenExpandPanel(){
        Length width = new Length(PANEL_WIDTH, LengthUnit.Percent);
        expPanel.style.width = new StyleLength(width);
        width = new Length(100 - PANEL_WIDTH, LengthUnit.Percent);
        expPanel.style.left = new StyleLength(width);
        expBody.style.display = DisplayStyle.Flex;

        expButton.text = "\u2192";
        isPanelExpanded = true;
    }

    public void PreRegisterCallback() {
        expButton.clicked += () => {
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
    }
    #endregion

    #region Tutorial

    public void BeginTutorial(TextAsset inkJSON){
        currStory = new Story(inkJSON.text);
        // dialogueVariables.StartListening(currentStory);
        
        tutIsPlaying = true;
        displaySpeakerName = "???";

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
        
        float targetValue = content.verticalScroller.highValue > 0 ? content.verticalScroller.highValue : 0;
        DOTween.To(()=>content.verticalScroller.value, x=> content.verticalScroller.value = x, targetValue, EXIT_LAG_TIME);

        HandleTags(currStory.currentTags);
    }

    private IEnumerator ExitTutorial(){
        yield return new WaitForSeconds(EXIT_LAG_TIME);
        
        // dialogueVariables.StopListening(currentStory);
        
        tutIsPlaying = false;
        displaySpeakerName = "";

        CloseExpandPanel();
    }

    private IEnumerator DisplayLine(string line){
        VisualElement textLine = textArea.Instantiate();
        Label label = textLine.Q<Label>();
        label.text = displaySpeakerName + "-";
        content.Add(textLine);

        canGoToNextLine = false;

        foreach (char letter in line.ToCharArray()){
            if (isUserInput()) {
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
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    public Ink.Runtime.Object GetVariableState(string varName){
        Ink.Runtime.Object variableValue = null;

        return variableValue;
    }

    public void OnApplicationQuit() {
        // dialogueVariables.SaveVariables();
    }

    private bool isUserInput(){
        return TutorialController.Instance.isInput;
    }

    #endregion
}
