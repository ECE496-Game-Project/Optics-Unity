using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Test
{
    public class Presenter
    {
        List<UnityAction<object>> m_actionReference = new List<UnityAction<object>>();
        List<FieldInfo> m_fieldInfos = new List<FieldInfo>();
        object m_curObject = null;

        


        public void Parsing(object o)
        {
            if (o == null) return;
            m_curObject = o;
            Type type = o.GetType();

            FieldInfo[] myFieldInfo = type.GetFields();



            for (int i = 0; i < myFieldInfo.Length; i++)
            {
                FieldInfo fi = myFieldInfo[i];

                Type filedType = fi.FieldType;

                string typeName = filedType.ToString();
                object field = fi.GetValue(o);

                // it must be Param<T>
                if (!typeName.Contains("Param")) continue;

                m_fieldInfos.Add(fi);
                Type genericArgument = fi.FieldType.GetGenericArguments()[0];

                // create the lambda expression that store in unity Action
                // redeclare the index variable because lambda expression capture by reference
                // if we use i, i will never get destroyed and always be the last value of i = myFieldInfo.Length
                int temp = i;
                UnityAction<object> newAction = (object info) => { SendInfo(info, temp); };

                // subscribe the corresponding variable event
                FieldInfo webEventFI = filedType.GetField("m_webEvent");

                object webEventObj = webEventFI.GetValue(field);

                Type webEventType = webEventFI.FieldType;

                MethodInfo addListenerMethod = webEventType.GetMethod("AddListener");

                // subscribe the event
                addListenerMethod.Invoke(webEventObj, new object[] { newAction });

                m_actionReference.Add(newAction);






            }
        }

        public void Clear()
        {
            for (int i = 0; i < m_fieldInfos.Count; i++)
            {
                FieldInfo fi = m_fieldInfos[i];
                Type filedType = fi.FieldType;
                object field = fi.GetValue(m_curObject);

                // subscribe the corresponding variable event
                FieldInfo webEventFI = filedType.GetField("m_webEvent");

                object webEventObj = webEventFI.GetValue(field);

                Type webEventType = webEventFI.FieldType;

                MethodInfo addListenerMethod = webEventType.GetMethod("RemoveListener");
                addListenerMethod.Invoke(webEventObj, new object[] { m_actionReference[i] });


            }

            m_actionReference.Clear();
            m_fieldInfos.Clear();
            m_curObject = null;
        }

        private void SendInfo(object info, int index)
        {
            // change the info variable to corresponding string

            // send the string to to the corresponding UI
            Debug.Log($"{info}: {index}");
        }


        public void ReceiveInfo(string info, int index)
        {
            if (index < 0 || index >= m_fieldInfos.Count)
            {
                Debug.Log($"Index invalid {index}");
                return;
            }

            FieldInfo fi = m_fieldInfos[index];
            Type filedType = fi.FieldType;
            object field = fi.GetValue(m_curObject);

            MethodInfo setValueMethod = filedType.GetMethod("SetValue");

            setValueMethod.Invoke(field, new object[] { info });
        }
    }



}

