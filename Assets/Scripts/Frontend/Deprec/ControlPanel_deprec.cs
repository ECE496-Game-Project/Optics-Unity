//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UIElements;
//using UnityEngine.SceneManagement;
//using System;
//using CommonUtils;

//namespace Panel {
//    public class SelectPanel : MonoBehaviour {
//        [SerializeField] private UIDocument _uiDocument;
//        [SerializeField] private StyleSheet _styleSheet;

//        private VisualElement _root;
//        private ListView _objectList;

//        private ListView _sceneList;
//        private VisualElement _content;


//        private void OnEnable() {
//            Generate();
//            RegisterEvent();
//        }

//        private void OnDisable() {
//            UnRegisterEvent();
//        }

//        #region UI Model
//        void Generate() {
//            if (_uiDocument == null || _styleSheet == null) return;

//            // level 0.
//            _root = _uiDocument.rootVisualElement;
//            _root.styleSheets.Add(_styleSheet);
//            _root.AddToClassList("root");
//            _root.AddToClassList("container");

//            // level 1.
//            var sideBar = new VisualElement();
//            sideBar.AddToClassList("sideBar");
//            sideBar.AddToClassList("container");
//            _root.Add(sideBar);

//            var currScene = new VisualElement();
//            currScene.AddToClassList("currScene");
//            currScene.AddToClassList("container");
//            _root.Add(currScene);

//            // level 2. 
//            // 2.1 Scene List
//            _sceneList = GenerateSideBar();
//            _sceneList.AddToClassList("sceneList");
//            sideBar.Add(_sceneList);

//            // 2.2 Current Scene 
//            var title = new Label("Scene Title");
//            title.AddToClassList("title");
//            title.AddToClassList("container");
//            currScene.Add(title);

//            _content = new VisualElement();
//            _content.AddToClassList("content");
//            _content.AddToClassList("container");
//            currScene.Add(_content);

//            // level 3.
//            _objectList = GenerateListView();
//            _objectList.AddToClassList("objectList");
//            _objectList.AddToClassList("container");
//            _content.Add(_objectList);
//        }
//        #endregion

//        #region Side Bar
//        ListView GenerateSideBar() {
//            ListView sceneList = new ListView {
//                makeItem = MakeSideBarItem,
//                bindItem = BindSideBarItem
//            };

//            int count = SceneManager.sceneCountInBuildSettings;
//            string[] sceneNames = new string[count];

//            for (int i = 0; i < count; i++) {
//                sceneNames[i] = SceneManager.GetSceneByBuildIndex(i).name;
//            }

//            sceneList.itemsSource = sceneNames;
//            return sceneList;
//        }

//        VisualElement MakeSideBarItem() {
//            var button = new Button();
//            button.styleSheets.Add(_styleSheet);
//            button.AddToClassList("sceneList__item");
//            return button;
//        }

//        void BindSideBarItem(VisualElement ve, int idx) {
//            Button button = ve as Button;
//            Label text = new Label(_sceneList.itemsSource[idx] as String);
//            button.Add(text);
//            button.RegisterCallback<ClickEvent, int>(LoadScene, idx);
//        }

//        #endregion

//        #region List View
//        ListView GenerateListView() {
//            ListView listView = new ListView();
//            GameObject[] objs = SceneManager.GetActiveScene().GetRootGameObjects();
//            List<GameObject> objList = new List<GameObject>();

//            for (int i = 0; i < objs.Length; i++) {
//                if (objs[i].gameObject.CompareTag("WaveTrack"))
//                {
//                    for (int j = 0; j < objs[i].transform.childCount; j++)
//                    {
//                        objList.Add(objs[i].transform.GetChild(j).gameObject);
//                    }
//                }
//                else
//                {
//                    objList.Add(objs[i]);
//                }
                
//            }
//            listView.itemsSource = objList;
//            return listView;
//        }

//        VisualElement MakeListViewItem() {
//            return new Label();
//        }

//        void BindListViewItem(VisualElement ve, int idx) {
//            Label label = ve as Label;
//            var obj = _objectList.itemsSource[idx] as GameObject;
//            label.text = obj.name;
//        }

//        #endregion


//        #region Events Controller

//        void RegisterEvent() {
//            // return a visual element, and assign it to _paramView based on the type of the selected listViewItem. 
//            _objectList.makeItem = MakeListViewItem;
//            _objectList.bindItem = BindListViewItem;
//            _objectList.selectionChanged += OnSelectlistViewItem;
//        }

//        void UnRegisterEvent() {

//        }
//        void OnSelectlistViewItem(IEnumerable<object> objects) {
//            int counter = 0;
//            foreach (object obj in objects) {
//                if (counter >= 1) {
//                    DebugLogger.Warning(this.name, "Selecting Multiple Objects!");
//                    break;
//                }
//                ParamControlPanel.Instance.SelectParamView(obj as GameObject);
//                counter++;
//            }
//        }

//        void LoadScene(ClickEvent evt, int index) {
//            try {
//                SceneManager.LoadScene(index);
//            }
//            catch {
//                Debug.LogWarning("Scene " + index + " does not exist.");
//            }
//        }
//        #endregion

//        #region Control Logic
//        #endregion
//    }
//}
