using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;

public class ParamControlPanel : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private StyleSheet _styleSheet;

    private VisualElement _root;
    private ListView _objectList;
    private ListView _sceneList;
    private VisualElement _paramView;

    private void OnEnable()
    {
        if(_uiDocument == null) 
            _uiDocument = gameObject?.GetComponent<UIDocument>();   
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
        // 2.1 Scene List
        _sceneList = GenerateSideBar();
        _sceneList.AddToClassList("sceneList");
        sideBar.Add(_sceneList);

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
        _objectList = GenerateListView();
        _objectList.AddToClassList("objectList");
        _objectList.AddToClassList("container");
        content.Add(_objectList);

        // _paramView = new VisualElement();
        _paramView = GenerateWaveSourceParam();
        _paramView.AddToClassList("paramView");
        _paramView.AddToClassList("container");
        content.Add(_paramView);
    }

    #region Side Bar

    ListView GenerateSideBar()
    {
        ListView sceneList = new ListView
        {
            makeItem = MakeSideBarItem,
            bindItem = BindSideBarItem
        };

        int count = SceneManager.sceneCountInBuildSettings;
        string[] sceneNames = new string[count];

        for(int i = 0; i < count; i++)
        {
            sceneNames[i] = SceneManager.GetSceneByBuildIndex(i).name;
        }

        sceneList.itemsSource = sceneNames;
        return sceneList;
    }

    VisualElement MakeSideBarItem()
    {
        var button = new Button();
        button.styleSheets.Add(_styleSheet);
        button.AddToClassList("sceneList__item");
        return button;
    }    

    void BindSideBarItem(VisualElement ve, int idx)
    {
        Button button = ve as Button;
        Label text = new Label(_sceneList.itemsSource[idx] as String);
        button.Add(text);
        button.RegisterCallback<ClickEvent, int>(LoadScene, idx);
    }

    #endregion

    #region List View
    ListView GenerateListView()
    {
        ListView listView = new ListView();
        GameObject[] objs = SceneManager.GetActiveScene().GetRootGameObjects();
        List<GameObject> objList = new List<GameObject>();
     
        for(int i = 0; i < objs.Length; i++)
        {
            objList.Add(objs[i]);
        }
        listView.itemsSource = objList;
        return listView;
    }

    VisualElement MakeListViewItem()
    {
        return new Label();
    }

    void BindListViewItem(VisualElement ve, int idx)
    {
        Label label = ve as Label;
        var obj = _objectList.itemsSource[idx] as GameObject;
        label.text = obj.name;
    }

    #endregion

    #region Param View

    void GenerateParamView()
    {
        // _paramView = GenerateWaveSourceParam();
        // _paramView = GeneratePolarizerParam();
        // _paramView = GenerateWavePlateParam();
    }

    /*
    目的：为了能够不hardcode所有不同Type的可展示Object（包括wave source, child wave, devices）
    方法：为所有需要展示的不同Type的Object，写一个统一的Interface
    要求：
    1. 能够用相同的Type去概括所有的可展示Object
    2. 所有实现该Interface的class，必须包含一个list，罗列所有需要展示的parameter。其中的信息包括：
    parameter的名字（required, string type），单位（required, string type, 若无单位则为“”），权限（required, Read&Write or ReadOnly），默认值（required, float type）, 上限（optional, float type），下限（optional, float type）。
    3. 所有实现该Interface的class，所列在list中的parameter，必须有符合其所写权限的读写权，如可以get和set，或只能get
    效果：
    1. UI侧可以根据该Interface去简单地判断Object是否可以被展示，且拓展性高
    2. UI侧不用HardCode所有不同的可展示Object的UI排版，而是可以Traverse List，然后程序化生成
        */

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
        param.AddToClassList("parameter__slider");
        var slide = new Slider(label, lowerBound, upperBound){
            value=defaultVal
        };
        param.Add(slide);
        
        var field = new VisualElement();
        field.AddToClassList("parameter__field");
        var num = new FloatField(){
            value=defaultVal
        };
        num.RegisterCallback<ChangeEvent<float>>(evt => LowerBoundCheck(evt, lowerBound));
        num.RegisterCallback<ChangeEvent<float>>(evt => UpperBoundCheck(evt, upperBound));
        field.Add(num);
        var uni = new Label(unit);
        field.Add(uni);
        param.Add(field);
        return param;
    }

    VisualElement GenerateParameter(string label, string unit, float bound, float defaultVal)
    {
        var param = new VisualElement();
        param.AddToClassList("parameter__field");
        var field = new FloatField(){
            label=label,
            value=defaultVal
        };
        field.RegisterCallback<ChangeEvent<float>>(evt => LowerBoundCheck(evt, bound));
        param.Add(field);
        var uni = new Label(unit);
        param.Add(uni);
        return param;
    }

    VisualElement GenerateParameter(string label, string unit, float defaultVal)
    {
        var param = new VisualElement();
        param.AddToClassList("parameter__field");
        var field = new FloatField(){
            label=label,
            value=defaultVal
        };
        param.Add(field);
        var uni = new Label(unit);
        param.Add(uni);
        return param;
    }
    #endregion

    #endregion

    #region Events Controller

    void RegisterEvent()
    {
        // return a visual element, and assign it to _paramView based on the type of the selected listViewItem. 
        _objectList.makeItem = MakeListViewItem;
        _objectList.bindItem = BindListViewItem;
        _objectList.selectionChanged += OnSelectlistViewItem;
    }

    void UnRegisterEvent()
    {

    }

    void LoadScene(ClickEvent evt, int index)
    {
        try{
            SceneManager.LoadScene(index);
        }catch{
            Debug.LogWarning("Scene " + index + " does not exist.");
        }
    }
   
    void OnSelectlistViewItem(IEnumerable<object> objects)
    {
        GenerateParamView();
    }

    void LowerBoundCheck(ChangeEvent<float> evt, float bound)
    {
        if(evt.newValue < bound)
        {
            var field = evt.currentTarget as FloatField;
            field.SetValueWithoutNotify(bound);
        }
    }

    void UpperBoundCheck(ChangeEvent<float> evt, float bound)
    {
        if(evt.newValue > bound)
        {
            var field = evt.currentTarget as FloatField;
            field.SetValueWithoutNotify(bound);
        }
    }

    #endregion

    #region Control Logic

    #endregion
}
