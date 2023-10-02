using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ParamsEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    private GameObject[] objList;
    private ListView listView;
    private Button refreshBtn;

    [MenuItem("Window/UI Toolkit/ParamsEditor")]
    public static void ShowExample()
    {
        ParamsEditor wnd = GetWindow<ParamsEditor>();
        wnd.titleContent = new GUIContent("ParamsEditor");
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        refreshBtn = root.Q<Button>("Params/unity-content/Refresh");
        refreshBtn.clicked += OnRefresh;
        listView = root.Q<ListView>("GOList");
    }

    private void OnRefresh()
    {
        Scene scene = SceneManager.GetActiveScene();
        objList = scene.GetRootGameObjects();
        listView.itemsSource = objList;
    }
}
