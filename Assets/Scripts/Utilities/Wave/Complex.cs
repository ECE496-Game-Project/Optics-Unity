using System;
using Complex = System.Numerics.Complex;

namespace WaveUtils {
    public class ComplexVector2 {
        public Complex[] Value;

        public override string ToString() {
            return Value[0].ToString().PadRight(20) + " | " + Value[1].ToString().PadLeft(20);
        }

        #region CONSTRUCTOR
        public ComplexVector2() {
            Value = new Complex[2];
        }
        public ComplexVector2(Complex a, Complex b) {
            Value = new Complex[2];
            Value[0] = a;
            Value[1] = b;
        }
        #endregion
    }

    public class ComplexMatrix2X2 {
        public Complex[,] Value;

        public override string ToString() {
            return Value[0,0] + " | " 
                + Value[0,1] + "\n"
                + Value[1,0] + " | " 
                + Value[1,1];
        }
        #region CONSTRUCTOR
        public ComplexMatrix2X2() {
            Value = new Complex[2,2];
        }
        public ComplexMatrix2X2(Complex a, Complex b, Complex c, Complex d) {
            Value = new Complex[2, 2];
            Value[0,0] = a;
            Value[0,1] = b;
            Value[1,0] = c;
            Value[1,1] = d;
        }
        #endregion
        #region OPERATOR
        public static ComplexVector2 operator *(ComplexMatrix2X2 matrix, ComplexVector2 vec) {
            ComplexVector2 res = new ComplexVector2();

            res.Value[0] = matrix.Value[0, 0] * vec.Value[0] + matrix.Value[0,1] * vec.Value[1];
            res.Value[1] = matrix.Value[1, 0] * vec.Value[0] + matrix.Value[1,1] * vec.Value[1];

            return res;
        }

        public static ComplexMatrix2X2 operator *(ComplexMatrix2X2 matrix1, ComplexMatrix2X2 matrix2) {
            ComplexMatrix2X2 res = new ComplexMatrix2X2();

            res.Value[0, 0] = matrix1.Value[0, 0] * matrix2.Value[0, 0] + matrix1.Value[0, 1] * matrix2.Value[1, 0];
            res.Value[0, 1] = matrix1.Value[0, 0] * matrix2.Value[0, 1] + matrix1.Value[0, 1] * matrix2.Value[1, 1];
            res.Value[1, 0] = matrix1.Value[1, 0] * matrix2.Value[0, 0] + matrix1.Value[1, 1] * matrix2.Value[1, 0];
            res.Value[1, 1] = matrix1.Value[1, 0] * matrix2.Value[0, 1] + matrix1.Value[1, 1] * matrix2.Value[1, 1];

            return res;
        }
        #endregion
    }
}