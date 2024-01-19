using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

public class ObjectViewPanel : MonoBehaviour
{
    public UIDocument doc;
    public const int PANEL_HEIGHT = 20;
    private const int HIDE_POSITION = 98;
    bool isPanelExpanded = false;

    public void PreRegisterCallback(VisualElement root) {
        Button expButton = root.Q<Button>(name: "ExpandButton");
        expButton.clicked += () => {
            if(isPanelExpanded) CloseExpandPanel(root);
            else OpenExpandPanel(root);
        };
    }

    void Awake()
    {
        VisualElement root = doc.rootVisualElement;
        CloseExpandPanel(root);
        PreRegisterCallback(root);
    }

    public void CloseExpandPanel(VisualElement root){
        VisualElement expPanel = root.Q<VisualElement>(name: "ExpandPanel");
        ScrollView expBody = root.Q<ScrollView>(name: "Body");
        Button expButton = root.Q<Button>(name: "ExpandButton");
        expPanel.style.height = PANEL_HEIGHT;
        expPanel.style.maxHeight = PANEL_HEIGHT;
        Length height = new Length(HIDE_POSITION, LengthUnit.Percent);
        expPanel.style.top = new StyleLength(height);
        expBody.style.display = DisplayStyle.None;
        expButton.text = "<";
        isPanelExpanded = false;
    }
    public void OpenExpandPanel(VisualElement root){
        VisualElement expPanel = root.Q<VisualElement>(name: "ExpandPanel");
        ScrollView expBody = root.Q<ScrollView>(name: "Body");
        Button expButton = root.Q<Button>(name: "ExpandButton");
        Length height = new Length(PANEL_HEIGHT, LengthUnit.Percent);
        expPanel.style.height = new StyleLength(height);
        expPanel.style.maxHeight = new StyleLength(height);
        height = new Length(100 - PANEL_HEIGHT, LengthUnit.Percent);
        expPanel.style.top = new StyleLength(height);
        expBody.style.display = DisplayStyle.Flex;
        expButton.text = ">";
        isPanelExpanded = true;
    }
}
