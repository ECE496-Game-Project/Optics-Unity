using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace Test { 
    public class TestObj<T>{
        public Subfield b = new Subfield();



    }

    public class Subfield
    {
        public void Test(int a)
        {
            Debug.Log($"I have been called {a}");
        }

        public void Test2 (string b)
        {
            Debug.Log($"Leo is Trash and {b}");
        }
    }

    public class BiggerObj
    {
        public TestObj<int> a = new TestObj<int>();
        public string d;
        public TestObj<float> c = new TestObj<float>();
    }




}