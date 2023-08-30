using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using Test;
using UnityEngine.Events;

public class ParamTest : MonoBehaviour
{

    List<UnityAction<object>> m_actionReference = new List<UnityAction<object>>();

    // Start is called before the first frame update
    void Start()
    {
        ParamObject testObj = new ParamObject();
        UnityEvent<int> Uevent = new UnityEvent<int>();
        UnityAction<object> act = (object a) => { Debug.Log(a); };

        Uevent.Invoke(3);

        //Uevent.RemoveListener(act);

        Uevent.Invoke(4);

        Parsing(testObj);

        testObj.a = 2;
        testObj.b.Value = 2.0f;
        testObj.c.Value = new Vector3(1, 2, 3);

    }


    private void Parsing(object o)
    {
        if (o == null) return;

        Type type = o.GetType();

        FieldInfo[] myFieldInfo = type.GetFields();

        

        for(int i = 0; i < myFieldInfo.Length; i++)
        {
            FieldInfo fi = myFieldInfo[i];
            Type filedType = fi.FieldType;
           
            string typeName = filedType.ToString();
            object field = fi.GetValue(o);

            // it must be Param<T>
            if (!typeName.Contains("Param")) continue;

            Type genericArgument = fi.FieldType.GetGenericArguments()[0];

            // create the lambda expression that store in unity Action
            // redeclare the index variable because lambda expression capture by reference
            // if we use i, i will never get destroyed and always be the last value of i
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

    private void SendInfo(object info, int index)
    {
        // change the info variable to corresponding string

        // send the string to to the corresponding UI
        Debug.Log($"{info}: {index}");
    }
}

public class ReflectionTest : MonoBehaviour
{

    private ReflectionObject m_reflectionObject;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 a = Vector3.one;
        a.SetValue("(1,1,1)");

        Param<ReflectionObject> test = new Param<ReflectionObject>();
        test.Value = new ReflectionObject();
        Type[] typeArguments = test.GetType().GetGenericArguments();

        foreach (Type type in typeArguments)
        {
            Debug.Log(type.Name);
        }

        m_reflectionObject = new ReflectionObject();
        m_reflectionObject.a = 1;
        m_reflectionObject.b = new Vector3(1, 2, 3);
        m_reflectionObject.c = 1.0f;

        PropertyInfo[] attributes = m_reflectionObject.GetType().GetProperties();

        foreach (PropertyInfo attribute in attributes)
        {
            var value = attribute.GetValue(m_reflectionObject);
            Debug.Log($"{attribute.Name}: {value}");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public class ReflectionObject
{
    public int a;
    public Vector3 b;
    public float c;

    public void test()
    {

    }
}

public static class ExtensionMethod
{


    public static void SetValue(this Vector3 vec, string a)
    {
        vec = new UnityEngine.Vector3(0, 0, 0);

    }
    public static void SetValue(this float vec, string a)
    {

    }


}


public class ParamObject
{
    public int a;
    public Param<float> b = new Param<float>(2f);
    public Param<Vector3> c = new Param<Vector3>(new Vector3(0,0,0));
}
