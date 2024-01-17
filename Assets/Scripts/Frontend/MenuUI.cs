using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using CommonUtils;

public class MenuUI : MonoBehaviour
{
    #region Common Page

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

        if(page_name == PAGENAME.Home) RenderHome(root);
        else if(page_name == PAGENAME.Free_Scene) RenderFreeScene(root);
        else if(page_name == PAGENAME.Tutorial) RenderTutorial(root);
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
            // header.Add(sectionButton.Instantiate());

            VisualElement secUI = sectionUI.Instantiate();
            Label sectionName = secUI.Q<Label>("name");
            sectionName.text = section.Key;
            content.Add(secUI);

            VisualElement sectionContent = secUI.Q("sectionContent");
            
            foreach(var course in section.Value){
                VisualElement crsButton = courseButton.Instantiate();
                Label courseName = crsButton.Q<Label>("name");
                courseName.text = course;
                VisualElement courseThumbnail = crsButton.Q("Thumbnail");
                courseThumbnail.style.backgroundImage = Resources.Load<Texture2D>($"Art/Images/Tutorial/{course}");
                // TODO: Register event to load corresponding scene
                sectionContent.Add(crsButton);
            }
         }
    }

    #endregion
}
