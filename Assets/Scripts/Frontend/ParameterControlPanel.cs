using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using ParameterTransfer;
using GO_Device;
using GO_Wave;
using CommonUtils;

/*
 * 
 * Parameter Portion of Control Pannel
 *
 */
namespace ControlPanel {
    public partial class ControlPanel : MonoBehaviour {
        private VisualElement _rootWSParamView;
        private VisualElement _WSParamView;
        private VisualElement _PDParamView;
        
        private VisualElement _paramView;

        private ParameterInfoList _rootWSInfo;
        private ParameterInfoList _WSInfo;
        private ParameterInfoList _PDInfo;

        private void EnableParamView() {
            if (_rootWSInfo == null)
                _rootWSInfo = CSVReader.ReadParametersCSV("Data/ParameterInfos/RootWaveParameters");

            if (_WSInfo == null)
                _WSInfo = CSVReader.ReadParametersCSV("Data/ParameterInfos/ChildWaveParameters");
            
            if (_PDInfo == null)
                _PDInfo = CSVReader.ReadParametersCSV("Data/ParameterInfos/PolarizedDeviceParameters");

            if (_uiDocument == null)
                _uiDocument = gameObject?.GetComponent<UIDocument>();
        }
        
        private VisualElement GenVEFromPIL(in ParameterInfoList infoList) {
            var ptr = new VisualElement();
            Stack<VisualElement> hier = new Stack<VisualElement>();
            string name = "";
            foreach (var entry in infoList.List) {
                switch (entry.Type) {
                    case ParamType.Hierarchy:
                        GenHierarchy(entry.Name, ref ptr, ref hier);
                        break;
                    case ParamType.HierarchyEnd:
                        GenHierarchyEnd(ref ptr, ref hier);
                        break;
                    case ParamType.String:
                        var stringF = GenText(entry.Name, (entry.Permit == Permission.RO));
                        ptr.Add(stringF);
                        break;
                    case ParamType.Int:
                        if (entry.Permit == Permission.RWEnum) {
                            ParameterInfo<DEVICETYPE> devicetypeEntry = entry as ParameterInfo<DEVICETYPE>;
                            if (devicetypeEntry.Name == "DEVICETYPE") {
                                name = entry.Symbol;
                                var enumF = new EnumField(name, devicetypeEntry.Default);
                                enumF.value = devicetypeEntry.Getter();
                                enumF.RegisterCallback(devicetypeEntry.Setter);
                                ptr.Add(enumF);
                            }
                        }
                        break;
                    case ParamType.Float:
                        name = entry.Name + "(" + entry.Symbol + ")";
                        ParameterInfo<float> floatEntry = entry as ParameterInfo<float>;
                        ParameterInfoBound<float> floatEntryBound = entry as ParameterInfoBound<float>;

                        FloatField floatF;
                        switch (entry.Permit) {
                            case Permission.RO:
                                floatF = GenFloat(name, entry.Unit);
                                ptr.Add(floatF);
                                break;
                            case Permission.RW:
                                floatF = GenFloat(name, entry.Unit, floatEntry.Default);
                                floatF.value = floatEntry.Getter();
                                floatF.RegisterCallback(floatEntry.Setter);
                                ptr.Add(floatF);
                                break;
                            case Permission.RWSlider:
                                floatF = GenFloat(
                                    name, entry.Unit, 
                                    floatEntryBound.Default, 
                                    floatEntryBound.UpperBound, 
                                    floatEntryBound.LowerBound);
                                floatF.value = floatEntryBound.Getter();
                                floatF.RegisterCallback(floatEntryBound.Setter);
                                ptr.Add(floatF);
                                break;
                            default:
                                DebugLogger.Error(this.name, "Unrecognized Info, break!");
                                break;
                        }
                        break;
                    default:
                        DebugLogger.Error(this.name, "Unrecognized Info, break!");
                        break;
                }
            }
            return ptr;
        }
        
        private void SelectParamView(GameObject obj) {
            
            /* Remove the previous showing Parameter View */
            if (_paramView != null) {
                _content.Remove(_paramView);
                _paramView = null;
            }

            RootWaveSource rws = obj.GetComponent<RootWaveSource>();
            if (rws != null) {
                rws.RegisterParametersCallback(_rootWSInfo);
                _paramView = GenVEFromPIL(_rootWSInfo);
                _content.Add(_paramView);
                return;
            }

            WaveSource ws = obj.GetComponent<WaveSource>();
            if (ws != null) {
                ws.RegisterParametersCallback(_WSInfo);
                _paramView = GenVEFromPIL(_WSInfo);
                _content.Add(_paramView);
                return;
            }

            PolarizedDevice pd = obj.GetComponent<PolarizedDevice>();
            if (pd != null) {
                pd.RegisterParametersCallback(_PDInfo);
                _paramView = GenVEFromPIL(_PDInfo);
                _content.Add(_paramView);
                return;
            }
        }

        VisualElement GenText(string name, bool isReadonly) {
            var text = new TextField(name);
            text.isReadOnly = isReadonly;
            return text;
        }

        void GenHierarchy(string name, ref VisualElement ptr, ref Stack<VisualElement> hier) {
            var fold = new Foldout() { text = name };
            ptr.Add(fold);
            hier.Push(ptr);
            ptr = fold;
        }

        void GenHierarchyEnd(ref VisualElement ptr, ref Stack<VisualElement> hier) {
            ptr = hier.Pop();
        }

        FloatField GenFloat(string name, string unit, float defaultVal, float lowerBound, float upperBound) {
            var param = new VisualElement();
            //param.AddToClassList("parameter__slider");
            var slide = new Slider(name, lowerBound, upperBound) {
                value = defaultVal
            };
            param.Add(slide);

            var field = new VisualElement();
            //field.AddToClassList("parameter__field");
            var num = new FloatField() {
                value = defaultVal
            };
            num.RegisterCallback<ChangeEvent<float>>(evt => LowerBoundCheck(evt, lowerBound));
            num.RegisterCallback<ChangeEvent<float>>(evt => UpperBoundCheck(evt, upperBound));
            field.Add(num);

            var uni = new Label(unit);
            field.Add(uni);
            param.Add(field);

            return num;
        }

        FloatField GenFloat(string label, string unit) {
            var param = new VisualElement();
            //param.AddToClassList("parameter__field");
            var field = new FloatField() {
                label = label,
                value = 0
            };
            field.isReadOnly = true;

            param.Add(field);
            var uni = new Label(unit);
            param.Add(uni);
            return field;
        }

        FloatField GenFloat(string label, string unit, float defaultVal) {
            var param = new VisualElement();
            //param.AddToClassList("parameter__field");
            var field = new FloatField() {
                label = label,
                value = defaultVal
            };

            param.Add(field);

            var uni = new Label(unit);
            param.Add(uni);
            return field;
        }

        void LowerBoundCheck(ChangeEvent<float> evt, float bound) {
            if (evt.newValue < bound) {
                var field = evt.currentTarget as FloatField;
                field.SetValueWithoutNotify(bound);
            }
        }

        void UpperBoundCheck(ChangeEvent<float> evt, float bound) {
            if (evt.newValue > bound) {
                var field = evt.currentTarget as FloatField;
                field.SetValueWithoutNotify(bound);
            }
        }
    }
}