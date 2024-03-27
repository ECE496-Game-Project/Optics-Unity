using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

using Ink.Runtime;
using CommonUtils;

public class TutorialPanel : MonoSingleton<TutorialPanel>
{
    [Header("Params")]
    [SerializeField] private float TYPE_SPEED = 0.04f;
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
    private VisualElement content;
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

    private void Awake()
    {
        root = doc.rootVisualElement;
        
        expPanel = root.Q<VisualElement>(name: "ExpandPanel");
        expBody = root.Q<VisualElement>(name: "Body");
        
        expButton = root.Q<Button>(name: "ExpandButton");
        pause = root.Q<Button>(name: "PauseButton");

        content = root.Q<VisualElement>(name: "content");
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
        // title.text = 
        tutIsPlaying = true;
        OpenExpandPanel();
        ContinueStory();
    }

    private void ContinueStory(){
        if(!currStory.canContinue){
            StartCoroutine(ExitTutorial());
            return;
        }

        // set text for the current dialogue line
        if(displayLine != null) StopCoroutine(displayLine); 
        displayLine = StartCoroutine(DisplayLine(currStory.Continue()));
        
        HandleTags(currStory.currentTags);
    }

    private IEnumerator ExitTutorial(){
        yield return new WaitForSeconds(EXIT_LAG_TIME);
        // dialogueVariables.StopListening(currentStory);
        tutIsPlaying = false;
        CloseExpandPanel();
    }

    private IEnumerator DisplayLine(string line){
        VisualElement textLine = textArea.Instantiate();
        Label label = textLine.Q<Label>();
        label.text = "";
        content.Add(textLine);

        canGoToNextLine = false;

        foreach (char letter in line.ToCharArray()){
            if (isUserInput()) {
                label.text = line;
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

        // 1. Real Choice
        List<VisualElement> realChoices = new List<VisualElement>();
        foreach(Choice choice in currChoices) {   
            VisualElement choiceElement = realChoice.Instantiate();
            realChoices.Add(choiceElement);
            content.Add(choiceElement);
        }

        int index = 0;
        foreach(Choice choice in currChoices){
            Button button = realChoices[index].Q<Button>();
            button.text = choice.text;
            button.clicked += () => {
                MakeChoice(choice, realChoices);
            };
            index++;
        }

        // 2. Fake Choice
        // CONTINUE + \u25B6
    }

    private void MakeChoice(Choice choice, List<VisualElement> choices){
        if (!canGoToNextLine) return;
        
        foreach(VisualElement choiceEl in choices){
            content.Remove(choiceEl);
        }

        VisualElement textLine = textArea.Instantiate();
        textLine.Q<Label>().text = choice.text;
        content.Add(textLine);

        currStory.ChooseChoiceIndex(choice.index);

        ContinueStory();
    }

    private void HandleTags(List<string> tags){

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
