using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using CommonUtils;

public class MenuUI : MonoBehaviour
{
    public enum PAGENAME {
        Home = 1,
        Free_Scene = 2,
        Tutorial = 3
    }

    public UIDocument doc;

    public PAGENAME page_name = PAGENAME.Home;

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

        if(page_name == PAGENAME.Home) RenderHome();
        else if(page_name == PAGENAME.Free_Scene) RenderFreeScene();
        else if(page_name == PAGENAME.Tutorial) RenderTutorial();
    }

    #region Home Page

    public void RenderHome(){

    }

    #endregion

    #region Free Scene Page

    public void RenderFreeScene(){
        
    }

    #endregion

    #region Tutorial Page

    public void RenderTutorial(){
        
    }

    #endregion
}
