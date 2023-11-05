using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

using System.Collections.Generic;
using System.Linq;

using GO_Device;

using GO_Wave;
using WaveUtils;

using UnityEngine.Assertions;
using System;

public class ParamUI : MonoBehaviour
{
    private WaveSource _waveSource;
    private Polarizer _polarizer;
    private Waveplate _waveplate;

    [SerializeField]
    private List<string> _componentsToDisplay = new List<string>
    {
        "RootWaveSource",
        "Polarizer",
        "Waveplate",
    };

    private Foldout _polarizerUI;
    private FloatField[] _polarizerFields = new FloatField[2];

    private Foldout _waveplateUI;
    private FloatField[] _waveplateFields = new FloatField[3];

    private Foldout _waveSourceUI;
    private TextField _name;
    private EnumField _type;
    private FloatField[] _E = new FloatField[2];
    private FloatField[] _WKN = new FloatField[3];
    private Slider[] _angle = new Slider[2];

    private List<GameObject> _objList = new List<GameObject>();
    private Dictionary<GameObject, string> _objDict = new Dictionary<GameObject, string>();
    private ListView _listView;
    private Button _refreshBtn;



    //##########################################UI Highlight Controller##########################################
    private UIHighlightController m_highlightController;
    #region Life Cycle

    private void OnEnable()
    {
        InitUIElements();
        RegisterCallbacks();
        ClearParamsUI();
        m_highlightController = GetComponent<UIHighlightController>();
        Assert.IsNotNull(m_highlightController);
    }

    private void OnDisable()
    {
        UnregisterCallbacks();
        m_highlightController.UnhighlightObject();
        
    }

    #endregion

    #region UI Elements 

    private void InitUIElements()
    {
        var uiDocument = GetComponent<UIDocument>();

        _polarizerUI = uiDocument.rootVisualElement.Q<Foldout>("Polarizer");
        _polarizerFields[0] = uiDocument.rootVisualElement.Q<FloatField>("ThicknessP");
        _polarizerFields[1] = uiDocument.rootVisualElement.Q<FloatField>("RotDeg");

        _waveplateUI = uiDocument.rootVisualElement.Q<Foldout>("Waveplate");
        _waveplateFields[0] = uiDocument.rootVisualElement.Q<FloatField>("ThicknessW");
        _waveplateFields[1] = uiDocument.rootVisualElement.Q<FloatField>("PlateDeg");
        _waveplateFields[2] = uiDocument.rootVisualElement.Q<FloatField>("AxisDiff");

        _waveSourceUI = uiDocument.rootVisualElement.Q<Foldout>("WaveSource");
        _name = uiDocument.rootVisualElement.Q<TextField>("Name");
        _type = uiDocument.rootVisualElement.Q<EnumField>("Type");
        _E[0] = uiDocument.rootVisualElement.Q<FloatField>("Eox");
        _E[1] = uiDocument.rootVisualElement.Q<FloatField>("Eoy");
        _WKN[0] = uiDocument.rootVisualElement.Q<FloatField>("W");
        _WKN[1] = uiDocument.rootVisualElement.Q<FloatField>("K");
        _WKN[2] = uiDocument.rootVisualElement.Q<FloatField>("N");
        _angle[0] = uiDocument.rootVisualElement.Q<Slider>("Theta");
        _angle[1] = uiDocument.rootVisualElement.Q<Slider>("Phi");

        _listView = uiDocument.rootVisualElement.Q<ListView>("GOList");
        _listView.makeItem = MakeListItem;
        _listView.bindItem = BindListItem;
        _listView.selectionChanged += OnSelectItem;

        _refreshBtn = uiDocument.rootVisualElement.Q<Button>("Refresh");
    }

    #endregion

    #region Model Value

    private void SetWaveSourceUIValues()
    {
        _name.value = _waveSource.gameObject.name;
        _type.value = _waveSource.ParameterGet<WAVETYPE>("Type");
        _E[0].value = _waveSource.ParameterGet<float>("Eox");
        _E[1].value = _waveSource.ParameterGet<float>("Eoy");
        _WKN[0].value = _waveSource.ParameterGet<float>("W");
        _WKN[1].value = _waveSource.ParameterGet<float>("K");
        _WKN[2].value = _waveSource.ParameterGet<float>("N");
        _angle[0].value = _waveSource.ParameterGet<float>("Theta");
        _angle[1].value = _waveSource.ParameterGet<float>("Phi");
    }

    private void SetPolarizerUIValues()
    {
        _polarizerFields[0].value = _polarizer.ParameterGet<float>("ThicknessOffset");
        _polarizerFields[1].value = _polarizer.ParameterGet<float>("RotDeg");
    }

    private void SetWaveplateUIValues()
    {
        _waveplateFields[0].value = _waveplate.ParameterGet<float>("ThicknessOffset");
        _waveplateFields[1].value = _waveplate.ParameterGet<float>("PlateDeg");
        _waveplateFields[2].value = _waveplate.ParameterGet<float>("AxisDiffDeg");
    }

    #endregion

    #region Events

    private void RegisterCallbacks()
    {
        _polarizerFields[0].RegisterCallback<ChangeEvent<float>>(OnThicknessChanged);
        _polarizerFields[1].RegisterCallback<ChangeEvent<float>>(OnRotDegChanged);

        _waveplateFields[0].RegisterCallback<ChangeEvent<float>>(OnThicknessChanged);
        _waveplateFields[1].RegisterCallback<ChangeEvent<float>>(OnPlateDegChanged);
        _waveplateFields[2].RegisterCallback<ChangeEvent<float>>(OnAxisDiffChanged);

        _name.RegisterCallback<ChangeEvent<string>>(OnNameChanged);
        _type.RegisterCallback<ChangeEvent<WAVETYPE>>(OnTypeChange);
        _E[0].RegisterCallback<ChangeEvent<float>>(OnEoxChanged);
        _E[1].RegisterCallback<ChangeEvent<float>>(OnEoyChanged);
        _WKN[0].RegisterCallback<ChangeEvent<float>>(OnWChanged);
        _WKN[1].RegisterCallback<ChangeEvent<float>>(OnKChanged);
        _WKN[2].RegisterCallback<ChangeEvent<float>>(OnNChanged);
        _angle[0].RegisterCallback<ChangeEvent<float>>(OnThetaChanged);
        _angle[1].RegisterCallback<ChangeEvent<float>>(OnPhiChanged);

        _refreshBtn.RegisterCallback<ClickEvent>(OnRefresh);
    }

    private void UnregisterCallbacks()
    {
        _polarizerFields[0].UnregisterCallback<ChangeEvent<float>>(OnThicknessChanged);
        _polarizerFields[1].UnregisterCallback<ChangeEvent<float>>(OnRotDegChanged);

        _waveplateFields[0].UnregisterCallback<ChangeEvent<float>>(OnThicknessChanged);
        _waveplateFields[1].UnregisterCallback<ChangeEvent<float>>(OnPlateDegChanged);
        _waveplateFields[2].UnregisterCallback<ChangeEvent<float>>(OnAxisDiffChanged);

        _name.UnregisterCallback<ChangeEvent<string>>(OnNameChanged);
        _type.UnregisterCallback<ChangeEvent<WAVETYPE>>(OnTypeChange);
        _E[0].UnregisterCallback<ChangeEvent<float>>(OnEoxChanged);
        _E[1].UnregisterCallback<ChangeEvent<float>>(OnEoyChanged);
        _WKN[0].UnregisterCallback<ChangeEvent<float>>(OnWChanged);
        _WKN[1].UnregisterCallback<ChangeEvent<float>>(OnKChanged);
        _WKN[2].UnregisterCallback<ChangeEvent<float>>(OnNChanged);
        _angle[0].UnregisterCallback<ChangeEvent<float>>(OnThetaChanged);
        _angle[1].UnregisterCallback<ChangeEvent<float>>(OnPhiChanged);

        _refreshBtn.UnregisterCallback<ClickEvent>(OnRefresh);
    }

    #region List View

    private void OnRefresh(ClickEvent evt)
    {
        _objList.Clear();
        _objDict.Clear();

        Scene scene = SceneManager.GetActiveScene();

        foreach(GameObject obj in scene.GetRootGameObjects())
        {
            Component[] components = obj.GetComponents<Component>();

            foreach(Component component in components)
            {
                string name = component.GetType().Name;
                if (_componentsToDisplay.Contains(name))
                {
                    _objList.Add(obj);
                    _objDict.Add(obj, name);
                    break;
                }
            }
        }

        _listView.itemsSource = _objList;
    }

    private VisualElement MakeListItem()
    {
        var label = new Label();
        return label;
    }

    private void BindListItem(VisualElement ve, int idx)
    {
        Label label = ve as Label;
        var obj = _listView.itemsSource[idx] as GameObject;
        label.text = obj.name;
    }

    private void OnSelectItem(IEnumerable<object> objects)
    {
        ClearParamsUI();
        m_highlightController.UnhighlightObject();

        foreach (GameObject obj in objects.Cast<GameObject>())
        {
            switch (_objDict[obj])
            {
                case "RootWaveSource":
                    _waveSource = obj.GetComponent<WaveSource>();
                    SetWaveSourceUIValues();
                    _waveSourceUI.style.display = DisplayStyle.Flex;
                    m_highlightController.HighlightObject(obj);
                    break;
                case "Polarizer":
                    _polarizer = obj.GetComponent<Polarizer>();
                    SetPolarizerUIValues();
                    _polarizerUI.style.display = DisplayStyle.Flex;
                    m_highlightController.HighlightObject(obj);
                    break;
                case "Waveplate":
                    _waveplate = obj.GetComponent<Waveplate>();
                    SetWaveplateUIValues();
                    _waveplateUI.style.display = DisplayStyle.Flex;
                    m_highlightController.HighlightObject(obj);
                    break;
                default:
                    break;
            }
        }
    }

    private void ClearParamsUI()
    {
        _waveSourceUI.style.display = DisplayStyle.None;
        _polarizerUI.style.display = DisplayStyle.None;
        _waveplateUI.style.display = DisplayStyle.None;
    }

    #endregion

    #region Params

    #region Wave Source

    private void OnNameChanged(ChangeEvent<string> evt)
    {
        _waveSource.gameObject.name = evt.newValue;
    }

    private void OnTypeChange(ChangeEvent<WAVETYPE> evt)
    {
        _waveSource.ParameterSet("Type", evt.newValue);
    }

    private void OnEoxChanged(ChangeEvent<float> evt)
    {
        _waveSource.ParameterSet("Eox", evt.newValue);
    }

    private void OnEoyChanged(ChangeEvent<float> evt)
    {
        _waveSource.ParameterSet("Eoy", evt.newValue);
    }

    private void OnWChanged(ChangeEvent<float> evt)
    {
        _waveSource.ParameterSet("W", evt.newValue);
    }

    private void OnKChanged(ChangeEvent<float> evt)
    {
        _waveSource.ParameterSet("K", evt.newValue);
    }

    private void OnNChanged(ChangeEvent<float> evt)
    {
        _waveSource.ParameterSet("N", evt.newValue);
    }

    private void OnThetaChanged(ChangeEvent<float> evt)
    {
        _waveSource.ParameterSet("Theta", evt.newValue);
    }

    private void OnPhiChanged(ChangeEvent<float> evt)
    {
        _waveSource.ParameterSet("Phi", evt.newValue);
    }

    #endregion

    #region Polarizer

    private void OnRotDegChanged(ChangeEvent<float> evt)
    {
        _polarizer.ParameterSet("RotDeg", evt.newValue);
    }

    private void OnThicknessChanged(ChangeEvent<float> evt)
    {
        _polarizer.ParameterSet("ThicknessOffset", evt.newValue);
    }

    #endregion

    #region Waveplate

    private void OnAxisDiffChanged(ChangeEvent<float> evt)
    {
        _waveplate.ParameterSet("AxisDiffDeg", evt.newValue);
    }

    private void OnPlateDegChanged(ChangeEvent<float> evt)
    {
        _waveplate.ParameterSet("PlateDeg", evt.newValue);
    }

    #endregion

    #endregion

    #endregion
}
