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
        
        private VisualElement _selectedParam;

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

            _rootWSParamView = GenVEFromPIL(_rootWSInfo);
            //_rootWSParamView = GenRootWSParamView();
            //_rootWSParamView.AddToClassList("paramView");
            //_rootWSParamView.AddToClassList("container");

            _WSParamView = GenVEFromPIL(_WSInfo);
            //_WSParamView.AddToClassList("paramView");
            //_WSParamView.AddToClassList("container");

            _PDParamView = GenVEFromPIL(_PDInfo);
            //_polarizerParamView.AddToClassList("paramView");
            //_polarizerParamView.AddToClassList("container");
        }
        
        private VisualElement GenVEFromPIL(in ParameterInfoList infoList) {
            var ptr = new VisualElement();
            Stack<VisualElement> hier = new Stack<VisualElement>();

            foreach (var entry in infoList.List) {
                switch (entry.Type) {
                    case ParamType.Hierarchy:
                        GenHierarchy(entry.Name, ref ptr, ref hier);
                        break;
                    case ParamType.HierarchyEnd:
                        GenHierarchyEnd(ref ptr, ref hier);
                        break;
                    case ParamType.String:
                        GenText(entry.Name, (entry.Permit == Permission.RO), ptr);
                        break;
                    case ParamType.Int:
                        if (entry.Permit == Permission.RWEnum) {
                            ParameterInfo<int> intEntry = entry as ParameterInfo<int>;
                            if (intEntry.Name == "DEVICETYPE") {
                                ptr.Add(new EnumField(entry.Symbol, (DEVICETYPE)intEntry.Default));
                            }
                        }
                        break;
                    case ParamType.Float:
                        string name = entry.Name + "(" + entry.Symbol + ")";
                        ParameterInfo<float> floatEntry = entry as ParameterInfo<float>;
                        ParameterInfoBound<float> floatEntryBound = entry as ParameterInfoBound<float>;

                        switch (entry.Permit) {
                            case Permission.RO:
                                GenFloat(name, entry.Unit, ptr);
                                break;
                            case Permission.RW:
                                GenFloat(name, entry.Unit, floatEntry.Default, ptr);
                                break;
                            case Permission.RWSlider:
                                GenFloat(
                                    name, entry.Unit, 
                                    floatEntryBound.Default, 
                                    floatEntryBound.UpperBound, 
                                    floatEntryBound.LowerBound, 
                                    ptr);
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
            if (_selectedParam != null) {
                _content.Remove(_selectedParam);
                _selectedParam = null;
            }

            RootWaveSource rws = obj.GetComponent<RootWaveSource>();
            if (rws != null) {
                _content.Add(_rootWSParamView);
                _selectedParam = _rootWSParamView;
                rws.RegisterParametersCallback(_rootWSInfo);

                return;
            }

            WaveSource ws = obj.GetComponent<WaveSource>();
            if (ws != null) {
                _content.Add(_WSParamView);
                _selectedParam = _WSParamView;
                ws.RegisterParametersCallback(_WSInfo);
                return;
            }

            PolarizedDevice pd = obj.GetComponent<PolarizedDevice>();
            if (pd != null) {
                _content.Add(_PDParamView);
                _selectedParam = _PDParamView;
                pd.RegisterParametersCallback(_PDInfo);
            }
        }

        void GenText(string name, bool isReadonly, in VisualElement ptr) {
            var text = new TextField(name);
            text.isReadOnly = isReadonly;
            ptr.Add(text);
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

        void GenFloat(string name, string unit, float defaultVal, float lowerBound, float upperBound, in VisualElement ptr) {
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

            ptr.Add(param);
        }

        void GenFloat(string label, string unit, float bound, float defaultVal, in VisualElement ptr) {
            var param = new VisualElement();
            //param.AddToClassList("parameter__field");
            var field = new FloatField() {
                label = label,
                value = defaultVal
            };
            field.RegisterCallback<ChangeEvent<float>>(evt => LowerBoundCheck(evt, bound));
            param.Add(field);
            var uni = new Label(unit);
            param.Add(uni);
            ptr.Add(param);
        }

        void GenFloat(string label, string unit, in VisualElement ptr) {
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
            ptr.Add(param);
        }

        void GenFloat(string label, string unit, float defaultVal, in VisualElement ptr) {
            var param = new VisualElement();
            //param.AddToClassList("parameter__field");
            var field = new FloatField() {
                label = label,
                value = defaultVal
            };

            param.Add(field);
            var uni = new Label(unit);
            param.Add(uni);
            ptr.Add(param);
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