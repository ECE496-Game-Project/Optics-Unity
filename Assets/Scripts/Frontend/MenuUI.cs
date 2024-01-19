using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using CommonUtils;
using Profiles;

public class MenuUI : MonoBehaviour
{
    #region Common Page

    public UIDocument doc;

    public SO_GameInfo gameInfo;

    public void PreRegisterCallback(VisualElement root) {
        Button homeButton = root.Q<Button>(name: "home");
        homeButton.clicked += () => {
            gameInfo.currPage = SO_GameInfo.PAGENAME.Home;
            RenderPage(root);
        };
        Button sceneButton = root.Q<Button>(name: "free-scenes");
        sceneButton.clicked += () => {
            gameInfo.currPage = SO_GameInfo.PAGENAME.Free_Scene;
            RenderPage(root);
        };
        Button tutButton = root.Q<Button>(name: "tutorial");
        tutButton.clicked += () => {
            gameInfo.currPage = SO_GameInfo.PAGENAME.Tutorial;
            RenderPage(root);
        };
    }
    
    void Awake()
    {
        VisualElement root = doc.rootVisualElement;
        PreRegisterCallback(root);
        RenderPage(root);
    }

    #endregion

    #region Page Switch

    void ClearContent(VisualElement root){
        VisualElement content = root.Q("content");
        content.Clear();
    }

    void RenderContent(VisualElement root){
        if(gameInfo.currPage == SO_GameInfo.PAGENAME.Home){
            RenderHome(root);
        } 
        else if(gameInfo.currPage == SO_GameInfo.PAGENAME.Free_Scene){
            RenderFreeScene(root);
        } 
        else if(gameInfo.currPage == SO_GameInfo.PAGENAME.Tutorial){
            RenderTutorial(root);
        } 
    }

    void RenderPage(VisualElement root){
        ClearContent(root);
        RenderContent(root);
    }

    #endregion

    #region Home Page

    public void RenderHome(VisualElement root){
        VisualElement content = root.Q("content");
        if(content == null){
            Debug.LogWarning("Not found content");
            return;
        }
        
        var logoUI = Resources.Load<VisualTreeAsset>("Art/Frontend/PageUI/HomePage/LogoUI");
        content.Add(logoUI.Instantiate());
    }

    #endregion

    #region Free Scene Page

    public void RenderFreeScene(VisualElement root){
        VisualElement content = root.Q("content");
        if(content == null){
            Debug.LogWarning("Not found content");
            return;
        }

        var addSceneUI = Resources.Load<VisualTreeAsset>("Art/Frontend/PageUI/FreeScenePage/AddSceneButtonUI");
        content.Add(addSceneUI.Instantiate());

        // TODO: register an event, click add scene ui to create a new scene ui & create a new scene
    }

    #endregion

    #region Tutorial Page
    
    Dictionary<string, List<string>> tutDict = new Dictionary<string, List<string>>{
        {"Wave", new List<string> {"Basics"}},
        {"Polarization", new List<string> {"Polarizer", "Waveplate"}}
    };

    public void crsRegisterCallback(Button crs, string name){
        crs.clicked += () => {
            SceneManagementUtil.LoadScene(name);
        };
    }

    public void RenderTutorial(VisualElement root){
        VisualElement content = root.Q("content");
        if(content == null){
            Debug.LogWarning("Not found content");
            return;
        }

        VisualElement header = root.Q("header");
        if(header == null){
            Debug.LogWarning("Not found header");
            return;
        }        

        var sectionUI = Resources.Load<VisualTreeAsset>("Art/Frontend/PageUI/TutorialPage/SectionUI");
        var sectionButton = Resources.Load<VisualTreeAsset>("Art/Frontend/PageUI/TutorialPage/SectionButtonUI");
        var courseButton = Resources.Load<VisualTreeAsset>("Art/Frontend/PageUI/TutorialPage/CourseButtonUI");

        foreach(var section in tutDict){
            // TODO: header.Add(sectionButton.Instantiate());
            VisualElement secUI = sectionUI.Instantiate();
            Label sectionName = secUI.Q<Label>("name");
            sectionName.text = section.Key;
            content.Add(secUI);

            VisualElement sectionContent = secUI.Q("sectionContent");
            
            foreach(var course in section.Value){
                VisualElement crs = courseButton.Instantiate();
                Label courseName = crs.Q<Label>("Name");
                courseName.text = course;
                Button crsButton = crs.Q<Button>("Thumbnail");
                crsButton.style.backgroundImage = Resources.Load<Texture2D>($"Art/Images/Tutorial/{course}");
                crsRegisterCallback(crsButton, course);
                sectionContent.Add(crs);
            }
         }
    }

    #endregion
}
