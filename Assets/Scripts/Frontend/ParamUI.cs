using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ParamUI : MonoBehaviour
{
    private GameObject[] objList;
    private ListView listView;
    private Button refreshBtn;

    private void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        listView = uiDocument.rootVisualElement.Q<ListView>("GOList") as ListView;
        refreshBtn = uiDocument.rootVisualElement.Q<Button>("Refresh") as Button;
        refreshBtn.RegisterCallback<ClickEvent>(OnRefresh);
    }

    private void OnDisable()
    {
        refreshBtn.UnregisterCallback<ClickEvent>(OnRefresh);
    }

    private void OnRefresh(ClickEvent evt)
    {
        Scene scene = SceneManager.GetActiveScene();
        objList = scene.GetRootGameObjects();
        listView.itemsSource = objList;
    }
}
