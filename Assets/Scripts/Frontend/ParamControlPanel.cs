using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParamControlPanel : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;

    private VisualElement _rootElement;
    private TreeView _treeView;
    private VisualElement _paramView;

    private void Awake()
    {
        if(_uiDocument == null) _uiDocument = gameObject?.GetComponent<UIDocument>();
        if(_uiDocument != null) _rootElement = _uiDocument.rootVisualElement;
        Generate();
        RegisterEvent();
    }

    #region UI Model

    void Generate ()
    {
        // 1. Title
        var title = new Label("Scene Title");
        _rootElement.Add(title);

        // 2. Tree View
        _treeView = new TreeView();
        _rootElement.Add(_treeView);

        // 3. Param View
        _paramView = new VisualElement();
        _rootElement.Add(_paramView);
    }

    VisualElement GenerateWaveSourceParam()
    {
        var waveSource = new VisualElement();

        var waveSourceTitle = new Label("Wave Source");
        waveSource.Add(waveSourceTitle);

        var nameField = new TextField("Name");
        waveSource.Add(nameField);

        var amplitude = new Foldout(); // "Amplitude(Eo)"
        waveSource.Add(amplitude);
        var eox = new Slider("X Amplitude(Eox)", -100, 100);
        amplitude.Add(eox);
        var eoy = new Slider("Y Amplitude(Eoy)", -100, 100);
        amplitude.Add(eoy);

        var temperal = new Foldout(); // "Temperal Properties"
        waveSource.Add(temperal);
        var temperalPeriod = new Slider("Period(T)", 0, 1000);
        temperal.Add(temperalPeriod);
        var temperalFreq = new Slider("Frequency(v)", 0, 1000);
        temperal.Add(temperalFreq);
        var temperalAgular = new Slider("Angular Frequency(w)", 0, 300);
        temperal.Add(temperalAgular);

        var spatial = new Foldout(); // "Spatial Properties"
        waveSource.Add(spatial);
        var spatialPeriod = new Slider("Period(Lambda)", 0, 1000);
        spatial.Add(spatialPeriod);
        var spatialFreq = new Slider("Frequency(f)", 0, 1000);
        spatial.Add(spatialFreq);
        var spatialAgular = new Slider("Angular Frequency(k)", 0, 300);
        spatial.Add(spatialAgular);
        
        var refractiveIndex = new Slider("Refractive Index(n)", 0, 1);
        waveSource.Add(refractiveIndex);
        var theta = new Slider("Phase Differece(Theta)", 0, 360);
        waveSource.Add(theta);

        return waveSource;
    }

    VisualElement GeneratePolarizerParam()
    {
        VisualElement polarizer = new VisualElement();
        
        var polarizerTitle = new Label("Polarizer");
        polarizer.Add(polarizerTitle);

        var nameField = new TextField("Name");
        polarizer.Add(nameField);

        var degree = new Slider("Rotation Degree", 0, 180);
        polarizer.Add(degree);

        return polarizer;
    }

    VisualElement GenerateWavePlateParam()
    {
        VisualElement wavePlate = new VisualElement();
        
        var wavePlateTitle = new Label("Wave Plate");
        wavePlate.Add(wavePlateTitle);

        var plateDegree = new Slider("Plate Degree", 0, 180);
        polarizer.Add(plateDegree);

        var axisDegree = new Slider("Axis Diff Degree", 0, 90);
        polarizer.Add(axisDegree);

        return wavePlate;
    }

    #endregion

    #region Event Controller

    void RegisterEvent()
    {
        // return a visual element, and assign it to _paramView based on the type of the selected TreeViewItem. 
        OnSelectItem();
    }

    void OnSelectItem()
    {
        _paramView = GenerateWaveSourceParam();
        // _paramView = GeneratePolarizerParam();
        // _paramView = GenerateWavePlateParam();
        _rootElement.Add(_paramView);
    }

    #endregion
}
