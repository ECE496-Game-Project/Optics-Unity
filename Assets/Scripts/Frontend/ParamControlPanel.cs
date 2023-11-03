using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ParamControlPanel : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private StyleSheet _styleSheet;

    private VisualElement _root;
    private ListView _listView;
    private VisualElement _paramView;

    private void OnEnable()
    {
        if(_uiDocument == null) _uiDocument = gameObject?.GetComponent<UIDocument>();   
        Generate();
        RegisterEvent();
    }

    private void OnDisable()
    {
        UnRegisterEvent();
    }

    #region UI Model

    void Generate ()
    {
        if(_uiDocument == null || _styleSheet == null) return;

        // level 0.
        _root = _uiDocument.rootVisualElement;
        _root.styleSheets.Add(_styleSheet);
        _root.AddToClassList("root");
        _root.AddToClassList("container");

        // level 1.
        var sideBar = new VisualElement();
        sideBar.AddToClassList("sideBar");
        sideBar.AddToClassList("container");
        _root.Add(sideBar);
        var currScene = new VisualElement();
        currScene.AddToClassList("currScene");
        currScene.AddToClassList("container");
        _root.Add(currScene);

        // level 2. 
        // 2.1 Side Bar (All Scenes)

        // 2.2 Current Scene 
        var title = new Label("Scene Title");
        title.AddToClassList("title");
        title.AddToClassList("container");
        currScene.Add(title);

        var content = new VisualElement();
        content.AddToClassList("content");
        content.AddToClassList("container");
        currScene.Add(content);
        
        // level 3.
        _listView = new ListView();
        _listView.AddToClassList("listView");
        _listView.AddToClassList("container");
        content.Add(_listView);

        _paramView = new VisualElement();
        _paramView.AddToClassList("paramView");
        _paramView.AddToClassList("container");
        content.Add(_paramView);
    }

    #region Side Bar

    #endregion

    #region List View

    #endregion

    #region Param View
    VisualElement GenerateWaveSourceParam()
    {
        var waveSource = new VisualElement();

        var title = new Label("Wave Source");
        title.AddToClassList("title");
        waveSource.Add(title);

        var nameField = new TextField("Name");
        waveSource.Add(nameField);

        var amplitude = new Foldout(){
            text = "Amplitude(Eo)"
        };
        waveSource.Add(amplitude);
        var eox = GenerateParameter("X Amplitude(Eox)", "V/nm", 10);
        amplitude.Add(eox);
        var eoy = GenerateParameter("Y Amplitude(Eoy)", "V/nm", 10);
        amplitude.Add(eoy);
        var theta = GenerateParameter("Phase Differece(Theta)", "Deg", -180, 180, 0);
        amplitude.Add(theta);

        var temperal = new Foldout(){
            text = "Temperal Properties"
        };
        waveSource.Add(temperal);
        var temperalPeriod = GenerateParameter("Period(T)", "fs", 0, 10);
        temperal.Add(temperalPeriod);
        var temperalAgular = GenerateParameter("Angular Frequency(w)", "rad/fs", 0, 10);
        temperal.Add(temperalAgular);
        var temperalFreq = new Label("Frequency(v) 10 THz");
        temperal.Add(temperalFreq);

        var spatial = new Foldout(){
            text = "Spatial Properties"
        };
        waveSource.Add(spatial);
        var spatialPeriod = GenerateParameter("Period(Lambda)", "nm", 0, 10);
        spatial.Add(spatialPeriod);
        var spatialAgular = GenerateParameter("Angular Frequency(k)", "rad/nm", 0, 10);
        spatial.Add(spatialAgular);
        var spatialFreq = new Label("Frequency(f) 10 nm^(-1)");
        spatial.Add(spatialFreq);
        
        var refractiveIndex = GenerateParameter("Refractive Index(n)", "", 1, 5, 1.5f);
        waveSource.Add(refractiveIndex);
        var phi = GenerateParameter("Initial Phase(Phi)", "Deg", -180, 180, 0);
        waveSource.Add(phi);

        return waveSource;
    }

    VisualElement GeneratePolarizerParam()
    {
        VisualElement polarizer = new VisualElement();
        
        var title = new Label("Polarizer");
        polarizer.Add(title);
        title.AddToClassList("title");

        var nameField = new TextField("Name");
        polarizer.Add(nameField);

        var degree = GenerateParameter("Rotation Degree", "Deg", 0, 180, 90);
        polarizer.Add(degree);

        return polarizer;
    }

    VisualElement GenerateWavePlateParam()
    {
        VisualElement wavePlate = new VisualElement();
        
        var title = new Label("Wave Plate");
        wavePlate.Add(title);
        title.AddToClassList("title");

        var nameField = new TextField("Name");
        wavePlate.Add(nameField);

        var plateDegree = GenerateParameter("Plate Degree", "Deg", 0, 180, 90);
        wavePlate.Add(plateDegree);

        var axisDegree = GenerateParameter("Axis Diff Degree", "Deg", 0, 90, 90);
        wavePlate.Add(axisDegree);

        return wavePlate;
    }

    VisualElement GenerateParameter(string label, string unit, int lowerBound, int upperBound, float defaultVal)
    {
        var param = new VisualElement();
        var slide = new Slider(label, lowerBound, upperBound){
            value=defaultVal
        };
        param.Add(slide);
        var field = new FloatField(){
            value=defaultVal
        };
        param.Add(field);
        if(unit != ""){
            var uni = new Label(unit);
            param.Add(uni);
        }
        return param;
    }

    VisualElement GenerateParameter(string label, string unit, float bound, float defaultVal)
    {
        var param = new VisualElement();
        var field = new FloatField(){
            label=label,
            value=defaultVal
        };
        field.RegisterCallback<ChangeEvent<float>>(evt => BoundValueCheck(evt, bound));
        param.Add(field);
        if(unit != ""){
            var uni = new Label(unit);
            param.Add(uni);
        }
        return param;
    }

    VisualElement GenerateParameter(string label, string unit, float defaultVal)
    {
        var param = new VisualElement();
        var field = new FloatField(){
            label=label,
            value=defaultVal
        };
        param.Add(field);
        if(unit != ""){
            var uni = new Label(unit);
            param.Add(uni);
        }
        return param;
    }
    #endregion

    #endregion

    #region Events Controller

    void RegisterEvent()
    {
        // return a visual element, and assign it to _paramView based on the type of the selected listViewItem. 
        OnSelectlistViewItem();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void UnRegisterEvent()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSelectlistViewItem()
    {
        LoadParamView();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadlistView();
    }

    void BoundValueCheck(ChangeEvent<float> evt, float bound)
    {
        if(evt.newValue < bound)
        {
            var field = evt.currentTarget as FloatField;
            field.SetValueWithoutNotify(bound);
        }
    }

    #endregion

    #region Control Logic

    void LoadParamView()
    {
         _paramView = GenerateWaveSourceParam();
        // _paramView = GeneratePolarizerParam();
        // _paramView = GenerateWavePlateParam();
        _root.Add(_paramView);
    }

    void LoadlistView()
    {
        GameObject[] objs = SceneManager.GetActiveScene().GetRootGameObjects();
        for(int i = 0; i < objs.Length; i++)
        {
            var item = new VisualElement(){
                name = objs[i].name
            };
            // _listView.AddItem();
        }
    }

    #endregion
}
