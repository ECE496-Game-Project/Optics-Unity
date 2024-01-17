using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

public class TutorialPanel : MonoBehaviour
{
    public UIDocument doc;
    public int PANEL_WIDTH = 30;
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
        VisualElement root = doc.rootVisualElement;;
        CloseExpandPanel(root);
        PreRegisterCallback(root);
    }

    void Update()
    {
        
    }

    #region Panel Animation
    public void CloseExpandPanel(VisualElement root){
        VisualElement expPanel = root.Q<VisualElement>(name: "ExpandPanel");
        VisualElement expBody = root.Q<VisualElement>(name: "Body");
        Button expButton = root.Q<Button>(name: "ExpandButton");

        expPanel.style.width = PANEL_WIDTH;
        Length width = new Length(98.5f, LengthUnit.Percent);
        expPanel.style.left = new StyleLength(width);
        expBody.style.display = DisplayStyle.None;

        expButton.text = "<";
        isPanelExpanded = false;
    }
    public void OpenExpandPanel(VisualElement root){
        VisualElement expPanel = root.Q<VisualElement>(name: "ExpandPanel");
        VisualElement expBody = root.Q<VisualElement>(name: "Body");
        Button expButton = root.Q<Button>(name: "ExpandButton");

        Length width = new Length(PANEL_WIDTH, LengthUnit.Percent);
        expPanel.style.width = new StyleLength(width);
        width = new Length(100 - PANEL_WIDTH, LengthUnit.Percent);
        expPanel.style.left = new StyleLength(width);
        expBody.style.display = DisplayStyle.Flex;

        expButton.text = ">";
        isPanelExpanded = true;
    }
    #endregion
}
