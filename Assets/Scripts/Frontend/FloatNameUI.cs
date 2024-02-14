using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatNameUI : MonoBehaviour
{
    private TextMeshProUGUI nameText;

    private void Awake() {
        nameText = transform.Find("Canvas/Text").GetComponent<TextMeshProUGUI>();
        if(nameText == null){
            Debug.LogError("No Text component found in children. Make sure it's a child of Canvas.");
            return;
        }
    }

    void UpdateIfNameChanged(){
        if(nameText.text != gameObject.name)
            nameText.text = gameObject.name;
    }

    private void OnEnable(){
        InvokeRepeating("UpdateIfNameChanged", 0f, 0.5f);
    }

    private void OnDisable(){
        CancelInvoke("UpdateIfNameChanged");
    }

    private void OnDestroy(){
        CancelInvoke("UpdateIfNameChanged");
    }
}
