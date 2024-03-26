using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using CommonUtils;
using Profiles;

public class SceneHeaderUI : MonoBehaviour
{
    public UIDocument doc;
    public SO_GameInfo gameInfo;
    public SO_GameInfo.PAGENAME escPage = SO_GameInfo.PAGENAME.Tutorial;

    private void PreRegisterCallback(VisualElement root) {
        Button escButton = root.Q<Button>(name: "EscButton");
        escButton.clicked += () => {
            gameInfo.currPage = escPage;
            SceneManagementUtil.LoadScene("Menu");
        };
    }

    private void RenderHeader(VisualElement root){
        Label title = root.Q<Label>(name: "Title");
        title.text = SceneManagementUtil.GetCurrSceneName();
    }

    void Awake()
    {
        VisualElement root = doc.rootVisualElement;
        RenderHeader(root);
        PreRegisterCallback(root);
    }
}
