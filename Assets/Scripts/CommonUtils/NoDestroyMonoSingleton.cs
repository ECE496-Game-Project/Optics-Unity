using UnityEngine;

namespace Optics.Utils
{
    /// <summary>
    /// Dont destroy on Load version monosingleton
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NoDestroyMonoSingleton<T> : MonoBehaviour where T: NoDestroyMonoSingleton<T>
    {
        private static bool isDestroyed = false;
        private static bool isDestroyedByMultipleInstance = false;
        private static T instance; 

        public static T Instance
        {
            get
            {
                if (isDestroyed) return null;
                if (instance != null) return instance;

                instance = FindObjectOfType<T>();

                if (instance == null) new GameObject("Singleton of "+typeof(T)).AddComponent<T>();
                else instance.Init(); 

                return instance;
            }
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (instance == null)
            {
                instance = this as T;
                Init();
            }
            else
            {
                isDestroyedByMultipleInstance = true;
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if(!isDestroyedByMultipleInstance) isDestroyed = true;
            isDestroyedByMultipleInstance = false;
        }

        protected virtual void Init()
        {

        }
    }
}