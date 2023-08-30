using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ExtensionMethods 
{
    public static void SetValue(this int val, string str)
    {
        val = int.Parse(str);
    }

    public static void SetValue(this float val, string str)
    {
        val = float.Parse(str);

       
    }
}
