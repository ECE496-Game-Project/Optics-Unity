using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
//using ParameterTransfer;
using GO_Device;
using GO_Wave;
using CommonUtils;

namespace Panel {
    
    public class ParamControlPanel : MonoSingleton<ParamControlPanel> {
        private Dictionary<string, VisualElement> _documentRoots;

        //private ParameterInfoList _rootWSInfo;
        //private ParameterInfoList _WSInfo;
        //private ParameterInfoList _PDInfo;

        bool isPanelExpanded = false;

        private void Awake() {
            //if (_rootWSInfo == null)
            //    _rootWSInfo = CSVReader.ReadParametersCSV("Data/ParameterInfos/RootWaveParameters");

            //if (_WSInfo == null)
            //    _WSInfo = CSVReader.ReadParametersCSV("Data/ParameterInfos/ChildWaveParameters");

            //if (_PDInfo == null)
            //    _PDInfo = CSVReader.ReadParametersCSV("Data/ParameterInfos/PolarizedDeviceParameters");
        }

        public void SelectParamView(GameObject obj) {

        }

        public void CleanParamView() {

        }
    }
}