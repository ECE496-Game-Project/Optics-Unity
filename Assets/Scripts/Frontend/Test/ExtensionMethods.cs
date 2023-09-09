using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class ExtensionMethods 
{
    public static Dictionary<Type, MethodInfo> m_extensionMethods;

    public static void Initialize()
    {
        Type type = typeof(ExtensionMethods);

        MethodInfo[] methods = type.GetMethods();
        m_extensionMethods = new Dictionary<Type, MethodInfo>();
        
        for (int i = 0; i < methods.Length; i++)
        {
            if (!methods[i].Name.Equals("SetValue")) continue;

            ParameterInfo parameters = methods[i].ReturnParameter;

            Type parameterType = parameters.ParameterType;

            m_extensionMethods.Add(parameterType, methods[i]);
        }
    }

    public static int SetValue(this int val, string str)
    {

        val =  int.Parse(str);
        return val;
    }

    public static float SetValue(this float val, string str)
    {
        val = float.Parse(str);
        return val;
    }
}
