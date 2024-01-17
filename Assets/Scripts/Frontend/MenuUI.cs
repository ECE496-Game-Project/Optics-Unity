using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using CommonUtils;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private enum pageName {
        Home = 1,
        Free_Scene = 2,
        Tutorial = 3
    }

    public UIDocument doc;

    public void PreRegisterCallback(VisualElement root) {
        Button homeButton = root.Q<Button>(name: "home");
        homeButton.clicked += () => {
            SceneManagementUtil.LoadScene("Home");
        };
        Button sceneButton = root.Q<Button>(name: "free-scenes");
        sceneButton.clicked += () => {
            SceneManagementUtil.LoadScene("FreeScene");
        };
        Button tutButton = root.Q<Button>(name: "tutorial");
        tutButton.clicked += () => {
            SceneManagementUtil.LoadScene("Tutorial");
        };
    }
    
    void Awake()
    {
        VisualElement root = doc.rootVisualElement;
        PreRegisterCallback(root);
    }

}
