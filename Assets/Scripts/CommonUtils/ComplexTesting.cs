using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using CommonUtils;

public class ComplexTesting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Complex c1 = new Complex(3, 2);
        Complex c2 = new Complex(1, 7);

        Complex c3 = Complex.Multiply(c1, c2);

        CustomComplex.Print(c3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
namespace CommonUtils {
    public static class CustomComplex {
        public static void Print(Complex c) {
            DebugLogger.Log("ComplexUtils", ToString(c));
            Complex.Exp(c);
        }

        public static string ToString(Complex c) {
            return (c.Real + " + " + c.Imaginary + "i");
        }
    }
}