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
    [SerializeField] private float EXIT_LAG_TIME = 0.2f;
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

    private Story currStory;
    private Coroutine displayLine;

    void Awake()
    {
        root = doc.rootVisualElement;
        
        expPanel = root.Q<VisualElement>(name: "ExpandPanel");
        expBody = root.Q<VisualElement>(name: "Body");
        
        expButton = root.Q<Button>(name: "ExpandButton");
        pause = root.Q<Button>(name: "PauseButton");

        OpenExpandPanel();
        PreRegisterCallback();

        content = root.Q<VisualElement>(name: "content");
        title = root.Q<Label>(name: "title");

        realChoice = Resources.Load<VisualTreeAsset>("Art/Frontend/Documents/TutorialPanel/RealChoice");
        fakeChoice = Resources.Load<VisualTreeAsset>("Art/Frontend/Documents/TutorialPanel/FakeChoice");
        textArea = Resources.Load<VisualTreeAsset>("Art/Frontend/Documents/TutorialPanel/TextArea");

        BeginTutorial(defaultInkJSON);
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
        CloseExpandPanel();
    }

    private IEnumerator DisplayLine(string line){
        VisualElement textLine = textArea.Instantiate();
        Label label = textLine.Q<Label>();
        label.text = "";
        content.Add(textLine);

        bool isAddingRichTextTag = false;

        foreach (char letter in line.ToCharArray()){
            // if the submit button is pressed, finish up displaying the line right away
            // if (InputManager.GetInstance().GetSubmitPressed()) {
                // dialogueText.maxVisibleCharacters = line.Length;
                // break;
            // }

            // check for rich text tag, if found, add it without waiting
            if (letter == '<' || isAddingRichTextTag) {
                isAddingRichTextTag = true;
                if (letter == '>') isAddingRichTextTag = false;
            }

            else {
                label.text += letter;
                yield return new WaitForSeconds(TYPE_SPEED);
            }
        }

        DisplayChoices();
    }

    private void DisplayChoices(){

    }

    private void MakeChoice(int choiceIdx){

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

    #endregion
}
