using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using ParameterTransfer;
using GO_Device;
using GO_Wave;
using CommonUtils;
using Profiles;
using System;

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
            public SO_ParamTransfer paramTrans;
            public UIDocument doc;
        }
        // For Inspector Registration Propose
        public List<UIPair> UIInfo;

        private Dictionary<string, ParameterInfoList> paramInfoDict = new Dictionary<string, ParameterInfoList>();

        // [TODO]: ExpandPanel
        bool isPanelExpanded = false;
        float PanelWidth = 0;

        private void Awake() {
            foreach(var UI in UIInfo) {
                var pil = new ParameterInfoList(UI.paramTrans.List, UI.doc.rootVisualElement);
                paramInfoDict.Add(UI.doc.gameObject.name, pil);

                PreRegisterCallback(pil);
                // [TODO]: ExpandPanel Starting Point
                PreRegisterCallback(UI.doc.rootVisualElement);
            }
        }


        public void PreRegisterCallback(ParameterInfoList pil) {
            // Special Handling for Slider
            foreach(var entry in pil.SymbolQuickAccess) {
                var pi = entry.Value;
                if(pi.Type == ParamType.Float && pi.Permit == Permission.RWSlider) {
                    var picast = pi as ParameterInfo<float>;
                    Slider slider = picast.Root.Q<Slider>();
                    FloatField floatField = picast.Root.Q<FloatField>();
                    if(slider == null || floatField == null) {
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
            // [TODO]: ExpandPanel
            //Button expButton = root.Q<Button>(name: "ExpandButton");
            //expButton.clicked += () => {
            //    isPanelExpanded = !isPanelExpanded;
            //    _expand_panel.style.width = isPanelExpanded ? 400f : 0f; // Adjust the width
            //    expButton.text = isPanelExpanded ? "<" : ">";
            //};

        }

        public void SelectParamView(GameObject obj) {

        }

        public void CleanParamView() {

        }
    }
}