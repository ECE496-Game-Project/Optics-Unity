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

    public void PreRegisterCallback(VisualElement root) {
        Button escButton = root.Q<Button>(name: "EscButton");
        escButton.clicked += () => {
            gameInfo.currPage = SO_GameInfo.PAGENAME.Tutorial;
            SceneManagementUtil.LoadScene("Menu");
        };
    }

    void Awake()
    {
        VisualElement root = doc.rootVisualElement;
        PreRegisterCallback(root);
    }
}
