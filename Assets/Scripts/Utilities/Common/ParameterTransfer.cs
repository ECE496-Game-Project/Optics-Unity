using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine;
using GO_Device;

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

    /* UI Side Information Storage */
    /* 1. Initalize a ParameterInfoList using SO_ParamTransfer */
    /* 2. Get this Root VisualElement from the UI Document */
    public class ParameterInfo<T> : ParameterInfoBase {
        public EventCallback<ChangeEvent<T>> Setter;
        public Func<T> Getter;

        public VisualElement root;

        public void SetCallback(EventCallback<ChangeEvent<T>> setter, Func<T> getter) {
            Setter = setter;
            Getter = getter;
        }

        public ParameterInfo(ParameterInfoBase pib) {
            Name = pib.Name;
            Type = pib.Type;
            Permit = pib.Permit;
        }
    }
   
    public class ParameterInfoList {
        public Dictionary<string, ParameterInfoBase> SymbolQuickAccess;

        public ParameterInfoList() {
            SymbolQuickAccess = new Dictionary<string, ParameterInfoBase>();
        }
    }
}