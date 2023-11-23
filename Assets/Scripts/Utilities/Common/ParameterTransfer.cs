using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;

namespace ParameterTransfer {

    public enum Permission {
        Invalid = -1,
        RO,
        RW,
        RWEnum,
        RWSlider
    }
    public enum ParamType {
        Invalid = -1,
        Hierarchy,
        HierarchyEnd,
        String,
        Int,
        Float,
        Vector3
    }
    public class ParameterInfoBase {
        public string Name;
        public string Symbol;
        public string Unit;
        public ParamType Type;
        public Permission Permit;
       
        public ParameterInfoBase(
            string name, ParamType type) {
            Name = name;
            Type = type;
        }

        public ParameterInfoBase(
            string name, string symbol, string unit,
            ParamType type, Permission permit) {
            Name = name;
            Symbol = symbol;
            Unit = unit;
            Type = type;
            Permit = permit;
        }

        public virtual string InfoToString() {
            return "Name: " + Name + "Symbol: " + Symbol + "Unit: " + Unit + "Type: "+Type;
        }
    }
    public class ParameterInfo<T> : ParameterInfoBase {
        public T Default;
        public EventCallback<ChangeEvent<T>> Setter;
        public Func<T> Getter;

        public void SetCallback(EventCallback<ChangeEvent<T>> setter, Func<T> getter) {
            Setter = setter;
            Getter = getter;
        }

        public ParameterInfo(
            string name, string symbol, string unit,
            ParamType type, Permission permit,
            T defaultVal)
            : base(name, symbol, unit, type, permit) {
            Default = defaultVal;
        }
    }
    public class ParameterInfoBound<T> : ParameterInfo<T> {
        public T UpperBound;
        public T LowerBound;

        public ParameterInfoBound(
            string name, string symbol, string unit,
            ParamType type, Permission permit,
            T defaultVal, T upperBound, T lowerBound)
            : base(name, symbol, unit, type, permit, defaultVal) {
            UpperBound = upperBound;
            LowerBound = lowerBound;
        }
    }
    public class ParameterInfoList {
        public List<ParameterInfoBase> List;
        public Dictionary<string, ParameterInfoBase> SymbolQuickAccess;
    }
    public class CSVReader {
        public static ParameterInfoList ReadParametersCSV(string filePath) {
            ParameterInfoList paramInfo = new ParameterInfoList();
            var List = paramInfo.List = new List<ParameterInfoBase>();
            var QuickAccess = paramInfo.SymbolQuickAccess = new Dictionary<string, ParameterInfoBase>();

            // Read all lines from the CSV file
            TextAsset dataFile = Resources.Load<TextAsset>(filePath);

            string[] lines = dataFile.text.Split(new char[] { '\n' });
            for (int i = 1; i < lines.Length - 1; i++) {
                // Split line into parts (assuming comma-separated)
                string[] cols = lines[i].Split(',');

                ParamType type;
                if (!Enum.TryParse(cols[0], out type)) type = ParamType.Invalid;
                if (type <= ParamType.HierarchyEnd) {
                    List.Add(new ParameterInfoBase(cols[2], type));
                    continue;
                }

                Permission permit = Permission.Invalid;
                Enum.TryParse(cols[1], out permit);
                if (permit <= Permission.RW) {
                    switch (type) {
                        case ParamType.String:
                            var pis = new ParameterInfo<string>(
                                cols[2], cols[3], cols[4], type, permit, cols[5]
                            );
                            List.Add(pis);
                            QuickAccess.Add(cols[3], pis);
                            break;
                        case ParamType.Float:
                            var pif = new ParameterInfo<float>(
                                cols[2], cols[3], cols[4], type, permit, float.Parse(cols[5])
                            );
                            List.Add(pif);
                            QuickAccess.Add(cols[3], pif);
                            break;
                    }
                    continue;
                }

                if (permit == Permission.RWSlider && type == ParamType.Float) {
                    var pifs = new ParameterInfoBound<float>(
                                cols[2], cols[3], cols[4], type, permit, float.Parse(cols[5]),
                                float.Parse(cols[6]), float.Parse(cols[7])
                            );
                    List.Add(pifs);
                    QuickAccess.Add(cols[3], pifs);
                }

                if (permit == Permission.RWEnum && type == ParamType.Int) {
                    var piie = new ParameterInfo<int>(
                                cols[2], cols[3], cols[4], type, permit, int.Parse(cols[5])
                            );
                    List.Add(piie);
                    QuickAccess.Add(cols[3], piie);
                }
            }
            return paramInfo;
        }
    }

}