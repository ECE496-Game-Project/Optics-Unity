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
                value = m_value;
                m_logicEvent?.Invoke(value);
                m_webEvent?.Invoke(value);
            }
        }

        [HideInInspector]
        public UnityEvent<T> m_logicEvent, m_webEvent;


        public Param(T value){
            m_value = value;
        }

        public Param()
        {
            m_value = new T();
        }

        public void SetValue(string str)
        {
            Type type = m_value.GetType();
            
            MethodInfo method = type.GetMethod("SetValue");
            if (method == null)
            {
                Debug.LogError($"Type {type} does not have SetValue(string str) function");
                return;
            }

            method.Invoke(m_value, new object[]{str});
            m_logicEvent?.Invoke(m_value);
        }
    }
}

