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
            Generate();
            RegisterEvent();
        }

        private void OnDisable() {
            UnregisterEvent();
        } 

        private void Generate(){
            #region level 1: container
            if (_uiDocument == null || _styleSheet == null) return;
            _root = _uiDocument.rootVisualElement;
            _root.styleSheets.Add(_styleSheet);
            _root.AddToClassList("root");

            _expand_panel = new VisualElement();
            _expand_panel.AddToClassList("container");
            _expand_panel.AddToClassList("expand-panel");
            _root.Add(_expand_panel);

            Button toggleButton = new Button() { text = "<" };
            toggleButton.AddToClassList("button");
            toggleButton.clicked += () => {
                isPanelExpanded = !isPanelExpanded;
                _expand_panel.style.width = isPanelExpanded ? 100f : 0f; // Adjust the width
                toggleButton.text = isPanelExpanded ? ">" : "<";
            };
            _root.Add(toggleButton);
            #endregion

            #region level 2: scene list view
            _scene_list = new ListView();
            int count = SceneManager.sceneCountInBuildSettings;
            string[] sceneNames = new string[count];
            for (int i = 0; i < count; i++) {
                sceneNames[i] = SceneManager.GetSceneByBuildIndex(i).name;
            }
            _scene_list.itemsSource = sceneNames;
            _root.Add(_scene_list);
            
            Button addSceneButton = new Button() {text = "+"};
            addSceneButton.AddToClassList("button");
            addSceneButton.clicked += () => {
                string newSceneName = "new optics lab";
                Scene newScene = SceneManager.CreateScene(newSceneName);
                if(newScene.IsValid()) SceneManager.LoadScene(newSceneName);
                else Debug.LogError("Failed to create a new scene");
            };
            _root.Add(addSceneButton);
            #endregion
        }

        private void RegisterEvent(){

        }

        private void UnregisterEvent(){

        }
        #endregion


    }
}