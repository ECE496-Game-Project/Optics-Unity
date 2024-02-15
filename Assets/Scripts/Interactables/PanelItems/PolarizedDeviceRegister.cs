using System;
using Interfaces;
using ParameterTransfer;

namespace GO_Device {
    public partial class PolarizedDevice : I_ParameterPanel {
        public string CorrespondingUIInfoName { get { return "PolarizedDevice"; } }

        public void ParameterChangeTrigger() {
            m_parent.ParameterChangeTrigger();
        }

        public void RegisterParametersCallback(ParameterInfoList ParameterInfos) {
            var NameTuple = (ParameterInfo<string>)ParameterInfos.SymbolQuickAccess["Name"];
            var DeviceTypeTuple = (ParameterInfo<Enum>)ParameterInfos.SymbolQuickAccess["DeviceType"];
            var RotDegTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["RotDeg"];
            var AxisDiffDegTuple = (ParameterInfo<float>)ParameterInfos.SymbolQuickAccess["AxisDiffDeg"];

            NameTuple.Getter = () => { return this.name; };
            DeviceTypeTuple.Getter = () => { return DeviceType; };
            RotDegTuple.Getter = () => { return RotDeg; };
            AxisDiffDegTuple.Getter = () => { return AxisDiffDeg; };

            NameTuple.Setter = (evt) => { this.name = evt.newValue; };
            DeviceTypeTuple.Setter = (evt) => { DeviceType = (DEVICETYPE)evt.newValue; ParameterChangeTrigger(); };
            RotDegTuple.Setter = (evt) => { RotDeg = evt.newValue; ParameterChangeTrigger(); };
            AxisDiffDegTuple.Setter = (evt) => { AxisDiffDeg = evt.newValue; ParameterChangeTrigger(); };
        }
    }
}