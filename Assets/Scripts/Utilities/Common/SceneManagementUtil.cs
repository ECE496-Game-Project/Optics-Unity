using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

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

        public static void CreateScene(){

        }

        public static void DeleteScene(){

        }
    }
}
