using System;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class TutorialVariables{
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }
    private Story globalVariablesStory;
    private const string SAVE_VAR_KEY = "INK_VARIABLES";

    public TutorialVariables(TextAsset loadGlobalsJSON){
        // create the story
        globalVariablesStory = new Story(loadGlobalsJSON.text);
        // if we have saved data, load it
        if (PlayerPrefs.HasKey(SAVE_VAR_KEY)){
            string jsonState = PlayerPrefs.GetString(SAVE_VAR_KEY);
            globalVariablesStory.state.LoadJson(jsonState);
        }

        // initialize the dictionary
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVariablesStory.variablesState){
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            Debug.Log("Initialized global dialogue variable: " + name + " = " + value);
        }
    }

    public void SaveVariables() {
        if (globalVariablesStory == null) return;
        // load the current state of all of our variables to the globals story
        VariablesToStory(globalVariablesStory);
        DataManager.SaveData(SAVE_VAR_KEY, globalVariablesStory.state.ToJson());
    }

    public string LoadVariables(string key) {
        return DataManager.LoadData(key);
    }

    public void StartListening(Story story) {
        // it's important that VariablesToStory is before assigning the listener!
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story story) {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    private void VariableChanged(string name, Ink.Runtime.Object value) {
        if (!variables.ContainsKey(name)) return;
        variables.Remove(name);
        variables.Add(name, value);
    }

    private void VariablesToStory(Story story) {
        foreach(KeyValuePair<string, Ink.Runtime.Object> variable in variables) {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }
}
