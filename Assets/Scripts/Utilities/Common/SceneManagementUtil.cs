using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEditor;

namespace CommonUtils
{
    public static class SceneManagementUtil
    {
        public static void LoadScene(int idx){
            try {
                SceneManager.LoadScene(idx);
            }
            catch {
                Debug.LogWarning("Scene " + idx + " does not exist.");
            }
        }

        public static void LoadScene(string name){
            try {
                SceneManager.LoadScene(name);
            }
            catch {
                Debug.LogWarning("Scene " + name + " does not exist.");
            }
        }

        public static string GetCurrSceneName(){
            string currSceneName = SceneManager.GetActiveScene().name;
            return currSceneName;
        }

        public static void CreateScene(){
            // The name of the new scene
            // string sceneName = "MyCustomScene";

            // The path to the template scene
            // string templateScenePath = "Assets/Scenes/Demo/FreeScenes/Template.unity";

            // Create a new scene
            // Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // Load the template scene
            // EditorSceneManager.OpenScene(templateScenePath, OpenSceneMode.Additive);

            // Save the new scene with the desired name
            // string newScenePath = "Assets/Scenes/Demo/FreeScenes/" + sceneName + ".unity";
            // EditorSceneManager.SaveScene(newScene, newScenePath);

            // Close the template scene
            // EditorSceneManager.CloseScene(EditorSceneManager.GetSceneByPath(templateScenePath), true);

            // Add the new scene to the build settings
            // int newSceneIndex = EditorBuildSettings.scenes.Length;
            // ArrayUtility.Add(ref EditorBuildSettings.scenes, new EditorBuildSettingsScene(newScenePath, true));

            // Save the changes to the build settings
            // EditorBuildSettings.scenes[newSceneIndex].enabled = true;
            // EditorSceneManager.SaveOpenScenes();
        }

        public static void DeleteScene(){

        }
    }
}
