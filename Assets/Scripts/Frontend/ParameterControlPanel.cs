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
        private VisualElement _polarizerParamView;
        private VisualElement _waveplateParamView;
        
        private VisualElement _selectedParam;

        private ParameterInfoList _rootWSInfo;
        private ParameterInfoList _WSInfo;

        private void EnableParamView() {
            if (_rootWSInfo == null)
                _rootWSInfo = CSVReader.ReadParametersCSV("Data/ParameterInfos/RootWaveParameters");

            if (_WSInfo == null)
                _WSInfo = CSVReader.ReadParametersCSV("Data/ParameterInfos/ChildWaveParameters");

            if (_uiDocument == null)
                _uiDocument = gameObject?.GetComponent<UIDocument>();

            _rootWSParamView = GenRootWSParamView();
            _rootWSParamView.AddToClassList("paramView");
            _rootWSParamView.AddToClassList("container");

            _polarizerParamView = GenePolarizerParamView();
            _polarizerParamView.AddToClassList("paramView");
            _polarizerParamView.AddToClassList("container");

            _waveplateParamView = GenerateWavePlateParam();
            _waveplateParamView.AddToClassList("paramView");
            _waveplateParamView.AddToClassList("container");
        }
        private void SelectParamView(GameObject obj) {
            /* Remove the previous showing Parameter View*/
            if(_selectedParam != null)
                _content.Remove(_selectedParam);

            RootWaveSource rws = obj.GetComponent<RootWaveSource>();
            if (rws != null) {
                _content.Add(_rootWSParamView);
                _selectedParam = _rootWSParamView;
            }

            PolarizedDevice pd = obj.GetComponent<PolarizedDevice>();
            if (pd != null) {
                if (pd.DeviceType == DEVICETYPE.POLARIZER) {
                    _content.Add(_polarizerParamView);
                    _selectedParam = _polarizerParamView;
                }
                else {
                    _content.Add(_waveplateParamView);
                    _selectedParam = _waveplateParamView;
                }
            }
        }

        /*
        目的：为了能够不hardcode所有不同Type的可展示Object（包括wave source, child wave, devices）
        方法：为所有需要展示的不同Type的Object，写一个统一的Interface
        要求：
        1. 能够用相同的Type去概括所有的可展示Object
        2. 所有实现该Interface的class，必须包含一个list，罗列所有需要展示的parameter。其中的信息包括：
        parameter的名字（required, string type），单位（required, string type, 若无单位则为“”），权限（required, Read&Write or ReadOnly），默认值（required, float type）, 上限（optional, float type），下限（optional, float type）。
        3. 所有实现该Interface的class，所列在list中的parameter，必须有符合其所写权限的读写权，如可以get和set，或只能get
        效果：
        1. UI侧可以根据该Interface去简单地判断Object是否可以被展示，且拓展性高
        2. UI侧不用HardCode所有不同的可展示Object的UI排版，而是可以Traverse List，然后程序化生成
            */

        VisualElement GenRootWSParamView() {
            var rootWaveSource = new VisualElement();

            var title = new Label("Wave Source");
            title.AddToClassList("title");
            rootWaveSource.Add(title);

            var nameField = new TextField("Name");
            rootWaveSource.Add(nameField);

            var amplitude = new Foldout() {
                text = "Amplitude(Eo)"
            };
            rootWaveSource.Add(amplitude);
            var eox = GenerateParameter("X Amplitude(Eox)", "V/nm", 10);
            amplitude.Add(eox);
            var eoy = GenerateParameter("Y Amplitude(Eoy)", "V/nm", 10);
            amplitude.Add(eoy);
            var theta = GenerateParameter("Phase Differece(Theta)", "Deg", -180, 180, 0);
            amplitude.Add(theta);

            var temperal = new Foldout() {
                text = "Temperal Properties"
            };
            rootWaveSource.Add(temperal);
            var temperalPeriod = GenerateParameter("Period(T)", "fs", 0, 10);
            temperal.Add(temperalPeriod);
            var temperalAgular = GenerateParameter("Angular Frequency(w)", "rad/fs", 0, 10);
            temperal.Add(temperalAgular);
            var temperalFreq = new Label("Frequency(v) 10 THz");
            temperal.Add(temperalFreq);

            var spatial = new Foldout() {
                text = "Spatial Properties"
            };
            rootWaveSource.Add(spatial);
            var spatialPeriod = GenerateParameter("Period(Lambda)", "nm", 0, 10);
            spatial.Add(spatialPeriod);
            var spatialAgular = GenerateParameter("Angular Frequency(k)", "rad/nm", 0, 10);
            spatial.Add(spatialAgular);
            var spatialFreq = new Label("Frequency(f) 10 nm^(-1)");
            spatial.Add(spatialFreq);

            var refractiveIndex = GenerateParameter("Refractive Index(n)", "", 1, 5, 1.5f);
            rootWaveSource.Add(refractiveIndex);
            var phi = GenerateParameter("Initial Phase(Phi)", "Deg", -180, 180, 0);
            rootWaveSource.Add(phi);

            return rootWaveSource;
        }

        VisualElement GenePolarizerParamView() {
            VisualElement polarizer = new VisualElement();

            var title = new Label("Polarizer");
            polarizer.Add(title);
            title.AddToClassList("title");

            var nameField = new TextField("Name");
            polarizer.Add(nameField);

            var degree = GenerateParameter("Rotation Degree", "Deg", 0, 180, 90);
            polarizer.Add(degree);

            return polarizer;
        }

        VisualElement GenerateWavePlateParam() {
            VisualElement wavePlate = new VisualElement();

            var title = new Label("Wave Plate");
            wavePlate.Add(title);
            title.AddToClassList("title");

            var nameField = new TextField("Name");
            wavePlate.Add(nameField);

            var plateDegree = GenerateParameter("Plate Degree", "Deg", 0, 180, 90);
            wavePlate.Add(plateDegree);

            var axisDegree = GenerateParameter("Axis Diff Degree", "Deg", 0, 90, 90);
            wavePlate.Add(axisDegree);

            return wavePlate;
        }

        VisualElement GenerateParameter(string label, string unit, int lowerBound, int upperBound, float defaultVal) {
            var param = new VisualElement();
            param.AddToClassList("parameter__slider");
            var slide = new Slider(label, lowerBound, upperBound) {
                value = defaultVal
            };
            param.Add(slide);

            var field = new VisualElement();
            field.AddToClassList("parameter__field");
            var num = new FloatField() {
                value = defaultVal
            };
            num.RegisterCallback<ChangeEvent<float>>(evt => LowerBoundCheck(evt, lowerBound));
            num.RegisterCallback<ChangeEvent<float>>(evt => UpperBoundCheck(evt, upperBound));
            field.Add(num);
            var uni = new Label(unit);
            field.Add(uni);
            param.Add(field);
            return param;
        }

        VisualElement GenerateParameter(string label, string unit, float bound, float defaultVal) {
            var param = new VisualElement();
            param.AddToClassList("parameter__field");
            var field = new FloatField() {
                label = label,
                value = defaultVal
            };
            field.RegisterCallback<ChangeEvent<float>>(evt => LowerBoundCheck(evt, bound));
            param.Add(field);
            var uni = new Label(unit);
            param.Add(uni);
            return param;
        }

        VisualElement GenerateParameter(string label, string unit, float defaultVal) {
            var param = new VisualElement();
            param.AddToClassList("parameter__field");
            var field = new FloatField() {
                label = label,
                value = defaultVal
            };
            param.Add(field);
            var uni = new Label(unit);
            param.Add(uni);
            return param;
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