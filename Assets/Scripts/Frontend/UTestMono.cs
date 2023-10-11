using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using Test;
using CommonUtils;

public class UTestMono: MonoBehaviour
{
    private void Start()
    {
        BiggerObj a = new BiggerObj();
        ReflectionUtilities.InvokeSubfieldFunction(a, null, "b", "Test", new object[] { 2 });
    }
}