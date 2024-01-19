using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using CommonUtils;

namespace Panel
{
    public class SceneSelectPanel : MonoSingleton<SceneSelectPanel>
    {
        [SerializeField] private UIDocument _uiDocument;
        [SerializeField] private StyleSheet _styleSheet;
        private VisualElement _root;
        private VisualElement _expand_panel;
        private ListView _scene_list;
        bool isPanelExpanded = false;

        # region Scene Select UI Life Cycle
        private void Awake(){

        }

        private void OnEnable(){
            if (_uiDocument == null || _styleSheet == null) return;
            _root = _uiDocument.rootVisualElement;
            _root.styleSheets.Add(_styleSheet);
            _root.AddToClassList("root");
            GenerateContainer();
            GenerateSceneList();
        }

        private void OnDisable() {

        } 

        private void GenerateContainer(){
            Button toggleButton = new Button() { text = "<" };
            toggleButton.AddToClassList("button");
            _root.Add(toggleButton);

            _expand_panel = new VisualElement();
            _expand_panel.AddToClassList("container");
            _expand_panel.AddToClassList("expand-panel");
            _root.Add(_expand_panel);

            toggleButton.clicked += () => {
                isPanelExpanded = !isPanelExpanded;
                _expand_panel.style.width = isPanelExpanded ? 100f : 0f; // Adjust the width
                toggleButton.text = isPanelExpanded ? ">" : "<";
            };
        }

        private void GenerateSceneList(){
            _scene_list = new ListView{
                makeItem = MakeSceneListItem,
                bindItem = BindSceneListItem
            };
            int count = SceneManager.sceneCountInBuildSettings;
            string[] sceneNames = new string[count];
            for (int i = 0; i < count; i++) {
                sceneNames[i] = SceneManager.GetSceneByBuildIndex(i).name;
            }
            _scene_list.itemsSource = sceneNames;
            _expand_panel.Add(_scene_list);

            Button addSceneButton = new Button() {text = "+"};
            addSceneButton.AddToClassList("button");
            addSceneButton.clicked += () => {
                string newSceneName = "new optics lab";
                Scene newScene = SceneManager.CreateScene(newSceneName);
                if(newScene.IsValid()) SceneManager.LoadScene(newSceneName);
                else Debug.LogError("Failed to create a new scene");
            };
            _expand_panel.Add(addSceneButton);
        }

        #endregion

        #region Scene State and List
        
        // scene state

        // show scene list

        // update scene list

        #endregion

        #region Switch a Scene 

        // click scene button
        VisualElement MakeSceneListItem(){
            var button = new Button();
            button.styleSheets.Add(_styleSheet);
            button.AddToClassList("scene-list-item");
            return button;
        }
        void BindSceneListItem(VisualElement ve, int idx){
            Button button = ve as Button;
            Label text = new Label(_scene_list.itemsSource[idx] as String);
            button.Add(text);
            button.RegisterCallback<ClickEvent, int>(LoadScene, idx);
        }

        void LoadScene(ClickEvent evt, int idx) {
            try {
                SceneManager.LoadScene(idx);
            }
            catch {
                Debug.LogWarning("Scene " + idx + " does not exist.");
            }
        }

        // switch scene, show the scene title in the screen

        // the color of the current scene is different

        #endregion

        #region Create a Scene

        // click + button

        // create a new scene

        // add it to the build setting

        // add it to the scene list

        #endregion

        #region Delete a Scene

        // click x button

        // pop-up the window, cancel or confirm

        // delete it from the build setting

        // delete it from the scene list

        #endregion
    }
}