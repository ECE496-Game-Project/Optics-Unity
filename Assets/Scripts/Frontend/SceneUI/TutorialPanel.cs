using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

public class TutorialPanel : MonoBehaviour
{
    public UIDocument doc;
    public const int PANEL_WIDTH = 30;
    private const float HIDE_POSITION = 98.5f;
    bool isPanelExpanded = false;
    int curPage = 1;
    int maxPage = 3;
    public void PreRegisterCallback(VisualElement root) {
        Button expButton = root.Q<Button>(name: "ExpandButton");
        expButton.clicked += () => {
            if(isPanelExpanded) CloseExpandPanel(root);
            else OpenExpandPanel(root);
        };

        Button prevPage = root.Q<Button>(name: "prevPage");
        prevPage.clicked += () => {
            if (curPage <= 1) return;
            SetPageDisplay(false, curPage);
            curPage--;
            SetPageDisplay(true, curPage);
        };

        Button nextPage = root.Q<Button>(name: "nextPage");
        nextPage.clicked += () => {
            if(curPage >= maxPage) return;
            SetPageDisplay(false, curPage);
            curPage++;
            SetPageDisplay(true, curPage);
        };
    }

    public void SetPageDisplay(bool display, int pgn) {
        VisualElement page = doc.rootVisualElement.Q<VisualElement>(name: ("page"+ pgn.ToString()));
        page.style.display = display ? DisplayStyle.Flex : DisplayStyle.None;
    }

    void Awake()
    {
        VisualElement root = doc.rootVisualElement;
        CloseExpandPanel(root);
        PreRegisterCallback(root);
    }

    public void CloseExpandPanel(VisualElement root){
        VisualElement expPanel = root.Q<VisualElement>(name: "ExpandPanel");
        VisualElement expBody = root.Q<VisualElement>(name: "Body");
        Button expButton = root.Q<Button>(name: "ExpandButton");

        expPanel.style.width = PANEL_WIDTH;
        Length width = new Length(HIDE_POSITION, LengthUnit.Percent);
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
}
