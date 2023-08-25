using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using Test;

public class ParamTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = new Vector3();
    }

    private void Parsing(object o)
    {
        if (o == null) return;

        Type type = o.GetType();

        FieldInfo[] myFieldInfo = type.GetFields();

        for(int i = 0; i < myFieldInfo.Length; i++)
        {
            FieldInfo fi = myFieldInfo[i];
            Debug.Log($"{myFieldInfo[i].Name}");
            
  
        }
    }
}

public class ParamObject
{
    public Param<int> a;
    public Param<float> b;
    public Param<Vector3> c;
}
