using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using ParameterTransfer;
using GO_Device;
using GO_Wave;
using CommonUtils;
using Profiles;
using System;
using Interfaces;
using UnityEngine.Assertions;

namespace Panel {
    public class ParamPanelManager : MonoSingleton<ParamPanelManager> {

        // ParameterTransfer as a ScriptableObject Combine with a UIDocument
        /* 
         * 1. Initalize a ParameterInfoList using SO_ParamTransfer, List<ParameterInfoBase> => List<ParameterInfo>
         * (Cannot create List of ParamInfo<T> Directly because of General Type) 
         */
        /* 2. Get this Root VisualElement from the UI Document */
        /* 3. When GameObject Selected, find the corresponding UI and Register its Getter & Setter */
        
        [Serializable] 
        public class UIPair {
            public string name;
            public SO_ParamTransfer paramTrans;
            public UIDocument doc;
        }
        /* For Inspector Registration Propose */
        public List<UIPair> UIInfoTransfer;

        public class UIInfo {
            public ParameterInfoList List;
            public GameObject GOUI;
            public VisualElement ExpandPanel;
            public VisualElement Body;

            public UIInfo(ParameterInfoList list, GameObject goui, VisualElement expPanel, VisualElement body) {
                List = list;
                GOUI = goui;
                ExpandPanel = expPanel;
                Body = body;
            }
        }
        /* For Runtime Information Propose */
        private Dictionary<string, UIInfo> paramInfoDict = new Dictionary<string, UIInfo>();

        // [TODO]: ExpandPanel
        private string selectedUI;
        public float PANEL_WIDTH;
        bool isPanelExpanded = false;

        #region Preprocess
        public void PreRegisterCallback(ParameterInfoList pil) {
            // Special Handling for Slider
            foreach (var entry in pil.SymbolQuickAccess) {
                var pi = entry.Value;
                if (pi.Type == ParamType.Float && pi.Permit == Permission.RWSlider) {
                    var picast = pi as ParameterInfo<float>;
                    Slider slider = picast.Root.Q<Slider>();
                    FloatField floatField = picast.Root.Q<FloatField>();
                    if (slider == null || floatField == null) {
                        DebugLogger.Warning(this.name, "Slider PreRegister Failed at: " + pi.Name);
                        continue;
                    }

                    // If statement prevent Damping
                    slider.RegisterValueChangedCallback(evt => {
                        if (floatField.value != evt.newValue) floatField.value = evt.newValue;
                    });
                    floatField.RegisterValueChangedCallback(evt => {
                        if (slider.value != evt.newValue) slider.value = evt.newValue;
                    });
                }
            }
        }

        public void PreRegisterCallback(VisualElement root) {
            Button expButton = root.Q<Button>(name: "ExpandButton");
            VisualElement _expand_panel = root.Q<Button>(name: "ExpandPanel");
            expButton.clicked += () => {
                isPanelExpanded = !isPanelExpanded;
                //_expand_panel.style.width = isPanelExpanded ? PANEL_WIDTH : 0f;
                expButton.text = isPanelExpanded ? "<" : ">";
            };

        }

        private void Awake() {
            foreach (var UI in UIInfoTransfer) {
                VisualElement root = UI.doc.rootVisualElement;
                ParameterInfoList pil = new ParameterInfoList(UI.paramTrans.List, root);
                VisualElement expandPanel = root.Q("ExpandPanel");
                VisualElement body = root.Q("Body");

                paramInfoDict.Add(UI.name, new UIInfo(pil, UI.doc.gameObject, expandPanel, body));

                PreRegisterCallback(pil);
                PreRegisterCallback(UI.doc.rootVisualElement);
            }
        }
        #endregion

        #region Runtime Update


        private void CleanSetter(string UIName) {
            ParameterInfoList list = paramInfoDict[UIName].List;
            foreach (var entry in list.SymbolQuickAccess) {
                var pi = entry.Value;
                switch (pi.Type) {
                    case ParamType.String:
                        var picastStr = pi as ParameterInfo<string>;
                        if(picastStr.Setter != null) picastStr.Root.Q<TextField>().UnregisterValueChangedCallback(picastStr.Setter);
                        picastStr.Setter = null;
                        break;
                    case ParamType.Float:
                        var picastF = pi as ParameterInfo<float>;
                        if (picastF.Setter != null) picastF.Root.Q<FloatField>().UnregisterValueChangedCallback(picastF.Setter);
                        picastF.Setter = null;
                        break;
                    default:
                        DebugLogger.Error(this.name, "CleanSetter Not Defined, Panic!");
                        break;
                }
            }
        }
        private void RegisterSetter(string UIName) {
            ParameterInfoList list = paramInfoDict[UIName].List;
            foreach (var entry in list.SymbolQuickAccess) {
                var pi = entry.Value;
                switch (pi.Type) {
                    case ParamType.String:
                        var picastStr = pi as ParameterInfo<string>;
                        picastStr.Root.Q<TextField>().RegisterValueChangedCallback(picastStr.Setter);
                        break;
                    case ParamType.Float:
                        var picastF = pi as ParameterInfo<float>;
                        picastF.Root.Q<FloatField>().RegisterValueChangedCallback(picastF.Setter);
                        break;
                    default:
                        DebugLogger.Error(this.name, "RegisterSetter Not Defined, Panic!");
                        break;
                }
            }
        }

        private void CallGetter(string UIName) {
            ParameterInfoList list = paramInfoDict[UIName].List;
            foreach (var entry in list.SymbolQuickAccess) {
                var pi = entry.Value;
                switch (pi.Type) {
                    case ParamType.String:
                        var picastStr = pi as ParameterInfo<string>;
                        picastStr.Root.Q<TextField>().value = picastStr.Getter.Invoke();
                        break;
                    case ParamType.Float:
                        var picastF = pi as ParameterInfo<float>;
                        picastF.Root.Q<FloatField>().value = picastF.Getter.Invoke();
                        break;
                    default:
                        DebugLogger.Error(this.name, "CallGetter Not Defined, Panic!");
                        break;
                }
            }
        }

        private void EnableUI(string UIName) {
            //[TODO]: ExpandPanel, add the Expand Animation here to close current Panel, open selected Panel
            foreach (var UI in paramInfoDict) {
                if (UI.Key == UIName) UI.Value.GOUI.SetActive(true);
                else UI.Value.GOUI.SetActive(false);
            }
        }

        private void UISetupPipeline(I_ParameterTransfer pt, string UIName) {
            CleanSetter(UIName);
            pt.RegisterParametersCallback(paramInfoDict[UIName].List);
            RegisterSetter(UIName);
            CallGetter(UIName);
            EnableUI(UIName);
        }

        public void SelectParamView(GameObject obj) {
            
            
            RootWaveSource rws = obj.GetComponent<RootWaveSource>();
            WaveSource ws = obj.GetComponent<WaveSource>();
            PolarizedDevice pd = obj.GetComponent<PolarizedDevice>();
            if (rws != null) {
                UISetupPipeline(rws, "RootWaveSource");
            }
            else if (ws != null) {
                UISetupPipeline(ws, "WaveSource");
            }
            else if (pd != null) {
                UISetupPipeline(pd, "PolarizedDevice");
            }
            else {
                DebugLogger.Warning(this.name, "Parameter of this object not defined, do nothing.");
                return;
            }
        }
    }
    #endregion
}