using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UnityEngine;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class TutorialTrigger : MonoBehaviour 
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    private void Trigger(){
        TutorialPanel.Instance.BeginTutorial(inkJSON);
    }
}   
