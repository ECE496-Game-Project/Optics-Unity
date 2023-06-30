using UnityEngine;

namespace CommonUtils {
    public static class DebugLogger
    {
        public static void Error(string objectName, string message)
        {
#if UNITY_EDITOR
            Debug.LogError("[" + objectName + "]: " + message);
            Debug.Break();
#endif
        }

        public static void Log(string objectName, string message)
        {
#if UNITY_EDITOR
            Debug.Log("[" + objectName + "]: " + message);
#endif
        }

        public static void Warning(string objectName, string message)
        {
#if UNITY_EDITOR
            Debug.LogWarning("[" + objectName + "]: " + message);
#endif
        }
    }
}
