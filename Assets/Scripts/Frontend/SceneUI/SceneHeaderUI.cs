using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using CommonUtils;

public class SceneHeaderUI : MonoBehaviour
{
    public UIDocument doc;
    public void PreRegisterCallback(VisualElement root) {
        Button escButton = root.Q<Button>(name: "EscButton");
        escButton.clicked += () => {
            SceneManagementUtil.LoadScene("Tutorial");
        };
    }

    void Awake()
    {
        VisualElement root = doc.rootVisualElement;
        PreRegisterCallback(root);
    }
}
