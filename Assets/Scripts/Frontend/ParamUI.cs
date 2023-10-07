using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using GO_Wave;
using WaveUtils;
using System.Collections.Generic;
using System.Linq;

public class ParamUI : MonoBehaviour
{
    [SerializeField]
    private WaveSource _waveSource;

    [SerializeField]
    private List<string> _componentsToDisplay = new List<string>
    {
        "RootWaveSource",
        "Polarizer",
        "Waveplate",
    };

    private TextField _name;
    private EnumField _type;
    private IntegerField[] _E = new IntegerField[2];
    private IntegerField[] _WKN = new IntegerField[3];
    private Slider[] _angle = new Slider[2];

    private List<GameObject> _objList = new List<GameObject>();
    private ListView _listView;
    private Button _refreshBtn;

    #region Life Cycle

    private void OnEnable()
    {
        InitUIElements();
        //SetUIValues();
        RegisterCallbacks();
    }

    private void OnDisable()
    {
        UnregisterCallbacks();
    }

    #endregion

    #region UI Elements 

    private void InitUIElements()
    {
        var uiDocument = GetComponent<UIDocument>();

        _name = uiDocument.rootVisualElement.Q<TextField>("Name");
        _type = uiDocument.rootVisualElement.Q<EnumField>("Type");

        _E[0] = uiDocument.rootVisualElement.Q<IntegerField>("Eox");
        _E[1] = uiDocument.rootVisualElement.Q<IntegerField>("Eoy");

        _WKN[0] = uiDocument.rootVisualElement.Q<IntegerField>("W");
        _WKN[1] = uiDocument.rootVisualElement.Q<IntegerField>("K");
        _WKN[2] = uiDocument.rootVisualElement.Q<IntegerField>("N");

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
        _type.value = _waveSource.Params.Type;
        _E[0].value = (int)_waveSource.Params.Eox;
        _E[1].value = (int)_waveSource.Params.Eoy;
        _WKN[0].value = (int)_waveSource.Params.W;
        _WKN[1].value = (int)_waveSource.Params.K;
        _WKN[2].value = (int)_waveSource.Params.N;
        _angle[0].value = _waveSource.Params.Theta;
        _angle[1].value = _waveSource.Params.Phi;
    }

    #endregion

    #region Events

    private void RegisterCallbacks()
    {
        _type.RegisterCallback<ChangeEvent<WAVETYPE>>(OnTypeChange);
        _E[0].RegisterCallback<ChangeEvent<int>>(OnEoxChanged);
        _E[1].RegisterCallback<ChangeEvent<int>>(OnEoyChanged);
        _WKN[0].RegisterCallback<ChangeEvent<int>>(OnWChanged);
        _WKN[1].RegisterCallback<ChangeEvent<int>>(OnKChanged);
        _WKN[2].RegisterCallback<ChangeEvent<int>>(OnNChanged);
        _angle[0].RegisterCallback<ChangeEvent<float>>(OnThetaChanged);
        _angle[1].RegisterCallback<ChangeEvent<float>>(OnPhiChanged);

        _refreshBtn.RegisterCallback<ClickEvent>(OnRefresh);
    }

    private void UnregisterCallbacks()
    {
        _type.UnregisterCallback<ChangeEvent<WAVETYPE>>(OnTypeChange);
        _E[0].UnregisterCallback<ChangeEvent<int>>(OnEoxChanged);
        _E[1].UnregisterCallback<ChangeEvent<int>>(OnEoyChanged);
        _WKN[0].UnregisterCallback<ChangeEvent<int>>(OnWChanged);
        _WKN[1].UnregisterCallback<ChangeEvent<int>>(OnKChanged);
        _WKN[2].UnregisterCallback<ChangeEvent<int>>(OnNChanged);
        _angle[0].UnregisterCallback<ChangeEvent<float>>(OnThetaChanged);
        _angle[1].UnregisterCallback<ChangeEvent<float>>(OnPhiChanged);
        _refreshBtn.UnregisterCallback<ClickEvent>(OnRefresh);
    }

    #region List View

    private void OnRefresh(ClickEvent evt)
    {
        _objList.Clear();

        Scene scene = SceneManager.GetActiveScene();

        foreach(GameObject obj in scene.GetRootGameObjects())
        {
            Component[] components = obj.GetComponents<Component>();
            bool toKeep = false;

            foreach(Component component in components)
            {
                string name = component.GetType().Name;
                if (_componentsToDisplay.Contains(name))
                {
                    toKeep = true;
                    break;
                }
            }

            if (toKeep)
            {
                _objList.Add(obj);
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
        var obj = _objList[idx];
        label.text = obj.name;
    }

    private void OnSelectItem(IEnumerable<object> objects)
    {
        foreach (GameObject obj in objects.Cast<GameObject>())
        {
            _waveSource = obj.GetComponent<WaveSource>();
            SetWaveSourceUIValues();
        }
    } 

    #endregion

    #region Params

    private void OnTypeChange(ChangeEvent<WAVETYPE> evt)
    {
        _waveSource.Params.Type = evt.newValue;
    }

    private void OnEoxChanged(ChangeEvent<int> evt)
    {
        _waveSource.Params.Eox = evt.newValue;
    }

    private void OnEoyChanged(ChangeEvent<int> evt)
    {
        _waveSource.Params.Eox = evt.newValue;
    }

    private void OnWChanged(ChangeEvent<int> evt)
    {
        _waveSource.Params.Eox = evt.newValue;
    }

    private void OnKChanged(ChangeEvent<int> evt)
    {
        _waveSource.Params.Eox = evt.newValue;
    }

    private void OnNChanged(ChangeEvent<int> evt)
    {
        _waveSource.Params.Eox = evt.newValue;
    }

    private void OnThetaChanged(ChangeEvent<float> evt)
    {
        _waveSource.Params.Theta = evt.newValue;
    }

    private void OnPhiChanged(ChangeEvent<float> evt)
    {
        _waveSource.Params.Phi = evt.newValue;
    }

    #endregion

    #endregion
}
