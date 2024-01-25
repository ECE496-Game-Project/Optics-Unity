using UnityEngine;
using UnityEngine.UIElements;

using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using ParameterTransfer;
using CommonUtils;
using Profiles;
using Interfaces;


namespace Panel {
    public class ParamPanelManager : MonoSingleton<ParamPanelManager> {

        // ParameterTransfer as a ScriptableObject Combine with a UIDocument
        /* 
         * 1. Initalize a ParameterInfoList using SO_ParamTransfer, 
         * List<ParameterInfoBase> => List<ParameterInfo>
         * (Cannot create List of ParamInfo<T> Directly because of General Type) 
         * 2. Get this Root VisualElement from the UI Document
         * 3. When GameObject Selected, find the corresponding UI and Register its Getter & Setter 
         */

        // Parameter UI Container
        public UIDocument doc;
        private VisualElement Body;
        private VisualElement ExpandPanel;
        private Button ExpandButton;

        [Serializable] 
        public class UIPair {
            [Tooltip("nameOfGO must be same as the GameObject registered ParamTransferName that have a Parameter UI")]
            public string paramTransferName;
            public SO_ParamTransfer paramTrans;
            public VisualTreeAsset paramUIAsset;
        }
        
        public List<UIPair> UIInfoTransfer; // For Inspector Registration Purpose

        public class UIInfo {
            public ParameterInfoList List;
            public VisualElement VEOfGO;

            public UIInfo(ParameterInfoList list, VisualElement paramOfGO) {
                List = list;
                VEOfGO = paramOfGO;
            }
        }

        /* For Runtime Information Purpose */
        private Dictionary<string, UIInfo> m_paramInfoDict = new Dictionary<string, UIInfo>();

        private string m_selectedUI = "";
        public int PANEL_WIDTH = 30;
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
            expButton.clicked += () => {
                if(isPanelExpanded) CloseExpandPanel();
                else OpenExpandPanel();
            };
        }

        private void Awake() {
            Body = doc.rootVisualElement.Q("Body");
            ExpandPanel = doc.rootVisualElement.Q("ExpandPanel");
            ExpandButton = doc.rootVisualElement.Q<Button>("ExpandButton");
            
            foreach (var UIInfo in UIInfoTransfer) {
                VisualElement Container = UIInfo.paramUIAsset.Instantiate();
                Container.name = UIInfo.paramTransferName;
                Body.Add(Container);
                Container.style.height = new StyleLength(new Length(100, LengthUnit.Percent));

                // Manually Set ParamContainer's Display to None
                Container.style.display = DisplayStyle.None;
            }
            
            CloseExpandPanel();
        }

        private void Start() {
            // PreRegistration
            var root = doc.rootVisualElement;

            foreach (var UIInfo in UIInfoTransfer) {
                var paramOfGO = root.Q(UIInfo.paramTransferName);
                Assert.IsNotNull(paramOfGO);

                ParameterInfoList pil = new ParameterInfoList(UIInfo.paramTrans.List, root);

                m_paramInfoDict.Add(UIInfo.paramTransferName, new UIInfo(pil, paramOfGO));

                PreRegisterCallback(pil);
                PreRegisterCallback(root);
            }
        }
        #endregion

        #region Runtime Update
        private void CleanSetter() {
            ParameterInfoList list = m_paramInfoDict[m_selectedUI].List;
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
                    case ParamType.Enum:
                        var picastEPD = pi as ParameterInfo<Enum>;
                        if (picastEPD.Setter != null) picastEPD.Root.Q<EnumField>().UnregisterValueChangedCallback(picastEPD.Setter);
                        picastEPD.Setter = null;
                        break;
                    default:
                        DebugLogger.Error(this.name, "CleanSetter Not Defined, Panic!");
                        break;
                }
            }
        }
        
        private void RegisterSetter() {
            ParameterInfoList list = m_paramInfoDict[m_selectedUI].List;
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
                    case ParamType.Enum:
                        var picastEPD = pi as ParameterInfo<Enum>;
                        picastEPD.Root.Q<EnumField>().RegisterValueChangedCallback(picastEPD.Setter);
                        picastEPD.Setter = null;
                        break;
                    default:
                        DebugLogger.Error(this.name, "RegisterSetter Not Defined, Panic!");
                        break;
                }
            }
        }

        public void CallGetter() {
            ParameterInfoList list = m_paramInfoDict[m_selectedUI].List;
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
                    case ParamType.Enum:
                        var picastE = pi as ParameterInfo<Enum>;
                        picastE.Root.Q<EnumField>().value = picastE.Getter.Invoke();
                        break;
                    default:
                        DebugLogger.Error(this.name, "CallGetter Not Defined, Panic!");
                        break;
                }
            }
        }

        private void UISetupPipeline(I_ParameterTransfer pt) {
            CleanSetter();
            pt.RegisterParametersCallback(m_paramInfoDict[m_selectedUI].List);
            CallGetter();
            RegisterSetter();
        }

        public void SelectParamView(GameObject obj) {
            var objts = obj.GetComponent<I_ParameterTransfer>();
            if (objts == null)
                DebugLogger.Error(this.name, "Pass in GameObject does not have Component I_ParamTrans, Panic!");

            m_selectedUI = objts.ParamTransferName;

            // All functionality operates with string m_selectedUI
            UISetupPipeline(objts);
            OpenParamUIDisplay();
            OpenExpandPanel();
        }

        public void CloseExpandPanel(){
            Body.style.display = DisplayStyle.None;
            ExpandPanel.style.width = PANEL_WIDTH;
            ExpandButton.text = ">";

            isPanelExpanded = false;
        }

        public void OpenExpandPanel(){
            Length width = new Length(PANEL_WIDTH, LengthUnit.Percent);
            ExpandPanel.style.width = new StyleLength(width);
            Body.style.display = DisplayStyle.Flex;
            ExpandButton.text = "<";

            isPanelExpanded = true;
        }

        public void OpenParamUIDisplay() {
            foreach(var UIInfo in m_paramInfoDict) {
                if(UIInfo.Key == m_selectedUI) {
                    UIInfo.Value.VEOfGO.style.display = DisplayStyle.Flex;
                }
                else UIInfo.Value.VEOfGO.style.display = DisplayStyle.None;
            }
        }

        public void CloseAllParamUIDisplay() {
            foreach (var UIInfo in m_paramInfoDict)
                UIInfo.Value.VEOfGO.style.display = DisplayStyle.None;
        }
        #endregion
    }
}