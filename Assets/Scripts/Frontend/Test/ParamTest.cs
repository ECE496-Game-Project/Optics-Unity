using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using Test;
using UnityEngine.Events;
using CommonUtils;

public class ParamTest : MonoBehaviour
{

    Presenter m_presenter = new Presenter();

    
    // Start is called before the first frame update
    void Start()
    {

        ExtensionMethods.Initialize();
        ParamObject testObj = new ParamObject();
        UnityEvent<int> Uevent = new UnityEvent<int>();
        UnityAction<object> act = (object a) => { Debug.Log(a); };

        m_presenter.Parsing(testObj);

        testObj.a = 2;
        testObj.b.Value = 2.0f;
        testObj.c.Value = new Vector3(1, 2, 3);

        Debug.Log($"Before: {testObj.b.Value}");
        

        m_presenter.ReceiveInfo("7", 0);
        Debug.Log($"After: {testObj.b.Value}");


        m_presenter.Clear();
        testObj.a = 5;
        testObj.b.Value = 3.0f;
        testObj.c.Value = new Vector3(4, 2, 3);

        ParamObject testObj2 = new ParamObject();
        m_presenter.Parsing(testObj2);

        testObj2.a = 5;
        testObj2.b.Value = 3.0f;
        testObj2.c.Value = new Vector3(4, 2, 4);
        m_presenter.Clear();
    }

    
}

public class ReflectionTest : MonoBehaviour
{

    private ReflectionObject m_reflectionObject;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 a = Vector3.one;
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


public class ParamObject
{
    public int a;
    public Param<float> b = new Param<float>(2f);
    public Param<Vector3> c = new Param<Vector3>(new Vector3(0,0,0));
}
