using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using GO_Device;
using CommonUtils;
using UnityEngine.Assertions;

namespace ParameterTransfer {

    public enum Permission {
        RO,
        RW,
        RWEnum,
        RWSlider
    }
    public enum ParamType {
        String,
        Int,
        Float,
        Vector3
    }

    [Serializable] 
    public class ParameterInfoBase {
        public string Name;
        public ParamType Type;
        public Permission Permit;
        
        public virtual string InfoToString() {
            return "Name: " + Name + "Type: "+Type;
        }
    }



# region Runtime Parameter Information Storage

    public class ParameterInfo<T> : ParameterInfoBase {
        public EventCallback<ChangeEvent<T>> Setter;
        public Func<T> Getter;

        public VisualElement Root;

        public void SetCallback(EventCallback<ChangeEvent<T>> setter, Func<T> getter) {
            Setter = setter;
            Getter = getter;
        }

        public ParameterInfo(ParameterInfoBase pib, VisualElement root) {
            Name = pib.Name;
            Type = pib.Type;
            Permit = pib.Permit;
            Root = root;
        }
    }
   
    public class ParameterInfoList {
        public Dictionary<string, ParameterInfoBase> SymbolQuickAccess = new Dictionary<string, ParameterInfoBase>();

        public ParameterInfoList(List<ParameterInfoBase> list, VisualElement root) {
            Assert.IsNotNull(list);
            Assert.IsNotNull(root);
            foreach (ParameterInfoBase p in list) {
                VisualElement ve = root.Q(p.Name);
                if (ve == null) {
                    DebugLogger.Warning("ParameterInfoList Casting", "Cannot Find: " + p.Name);
                    continue;
                }
                switch (p.Type) {
                    case ParamType.String:
                        SymbolQuickAccess.Add(p.Name, new ParameterInfo<string>(p, ve));
                        break;
                    case ParamType.Int:
                        SymbolQuickAccess.Add(p.Name, new ParameterInfo<int>(p, ve));
                        break;
                    case ParamType.Float:
                        SymbolQuickAccess.Add(p.Name, new ParameterInfo<float>(p, ve));
                        break;
                }
            }
        }
    }
#endregion


}