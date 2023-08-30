using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using UnityEngine.Events;

namespace Test{



    [Serializable]
    public class Param<T> where T: new()
    {
        [SerializeField]
        private T m_value;
        public T Value
        {
            get => m_value;
            set
            {
                m_value = value;
                m_logicEvent?.Invoke(value);
                m_webEvent?.Invoke(value);
            }
        }

        [HideInInspector]
        public UnityEvent<T> m_logicEvent;
        public UnityEvent<object> m_webEvent;


        public Param(T value){
            m_value = value;
            m_logicEvent = new UnityEvent<T>();
            m_webEvent = new UnityEvent<object>();
        }

        public Param()
        {
            
            m_value = new T();
            m_logicEvent = new UnityEvent<T>();
            m_webEvent = new UnityEvent<object>();
        }

        public void SetValue(string str)
        {
            Type type = m_value.GetType();

            MethodInfo method;
            ExtensionMethods.m_extensionMethods.TryGetValue(type, out method);
            if (method == null)
            {
                Debug.LogError($"Type {type} does not have SetValue(string str) function");
                return;
            }

            
            m_value = (T) method.Invoke(null, new object[]{m_value, str});
            m_logicEvent?.Invoke(m_value);
        }
    }
}

