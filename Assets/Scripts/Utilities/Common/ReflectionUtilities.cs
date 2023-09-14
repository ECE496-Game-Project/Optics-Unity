using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace CommonUtils
{
    public static class ReflectionUtilities
    {
        public static List<FieldInfo> GetAllFieldInfos(object obj)
        {
            if (obj == null) return null;
            Type type = obj.GetType();

            List<FieldInfo> list = new List<FieldInfo>(type.GetFields());

            return list;
        }

        /// <summary>
        /// Invoke the method in the subfield of every field in the object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filter">only invoke the subfield method for the field that has certain pattern </param>
        /// <param name="subField">name of the subfield in every field</param>
        /// <param name="method">name of the method in the subfield you want to invoke</param>
        /// <param name="argument">argument that you want to pass to the argument</param>
        public static void InvokeSubfieldFunction(object obj, string filter, string subField, string method, object[] argument)
        {
            if (obj == null) return;
            Type type = obj.GetType();

            FieldInfo[] myFieldInfos = type.GetFields();

            for (int i = 0; i < myFieldInfos.Length; i++)
            {
                FieldInfo fi = myFieldInfos[i];

                Type fieldType = fi.FieldType;
                
                string typeName = fieldType.ToString();
                object field = fi.GetValue(obj);


                if (filter != null && !typeName.Contains(filter)) continue;

                // get the corresponding subfield in every field
                FieldInfo subFieldInfo;
                if (subField == null) subFieldInfo = fi;
                else subFieldInfo = fieldType.GetField(subField);

                if (subFieldInfo == null)
                {
                #if DEBUG
                    Debug.Log($"{i}th field {fi.Name} in the class {type} does not subfield {subField}");

                #endif
                    continue;
                }

                object subFieldObj = subFieldInfo?.GetValue(field);

                Type subFieldType = subFieldInfo.FieldType;

                // get the method in the subfield and invoke it
                MethodInfo methodInfo = subFieldType.GetMethod(method);
                
                #if DEBUG
                if (methodInfo == null) {
                    Debug.Log($"subfield {subField} in {i}th field {fi.Name} in the class {type} does not have method {method}");
                    
                }
                #endif
                // subscribe the event
                methodInfo?.Invoke(subFieldObj, argument);
            }
        }
    }
}
