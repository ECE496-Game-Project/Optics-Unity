using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SceneSelectionMenu : MonoBehaviour
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
            selections.Add(scene);
            
            Button thumbnail = new Button();
            thumbnail.AddToClassList("thumbnail");
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
}