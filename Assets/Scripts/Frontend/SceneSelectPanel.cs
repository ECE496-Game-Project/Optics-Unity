using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class SceneSelectPanel : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private StyleSheet _styleSheet;
    [SerializeField] private List<string> _selectionList = new List<string>
    {
        "Polarizer",
        "Waveplate",
        "Reflection",
    };
    private VisualElement _root;

    private void OnEnable()
    {
        if(_uiDocument == null) 
            _uiDocument = gameObject?.GetComponent<UIDocument>();
        Generate();
        RegisterEvent();
    }

    void Generate()
    {
        if(_uiDocument == null || _styleSheet == null) return;

        _root = _uiDocument.rootVisualElement;
        _root.styleSheets.Add(_styleSheet);
        _root.AddToClassList("root");

        var header = new VisualElement();
        header.AddToClassList("header");
        _root.Add(header);

        var title = new Label("Optics");
        title.AddToClassList("title");
        header.Add(title);

        var content = new VisualElement();
        content.AddToClassList("content");
        _root.Add(content);
        
        var selection = GenerateSelection();
        content.Add(selection);
    }

    VisualElement GenerateSelection()
    {
        VisualElement selections = new VisualElement();
        selections.AddToClassList("scenes");

        for(int i = 0; i < _selectionList.Count; i++)
        {
            VisualElement scene = new VisualElement();
            scene.AddToClassList("scene");
            selections.Add(scene);
            
            Button thumbnail = new Button();
            thumbnail.AddToClassList("thumbnail");
            thumbnail.style.backgroundImage = Resources.Load<Texture2D>($"Art/Images/{_selectionList[i]}");
            thumbnail.RegisterCallback<ClickEvent, string>(LoadScene,  _selectionList[i]);
            scene.Add(thumbnail);

            Label label = new Label(_selectionList[i]);
            label.AddToClassList("name");
            scene.Add(label);
        }

        return selections;
    }

    void RegisterEvent()
    {

    }

    void LoadScene(ClickEvent evt, string scene)
    {
        try
        {
            SceneManager.LoadScene(scene);
        }
        catch
        {
            Debug.LogWarning("Scene " + scene + " does not exist.");
        }
    }
}