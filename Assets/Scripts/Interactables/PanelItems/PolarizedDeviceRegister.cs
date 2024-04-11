using System;
using Interfaces;
using ParameterTransfer;
using GO_Wave;
using UnityEngine;
using Panel;

namespace GO_Device {
    public partial class PolarizedDevice : I_ParameterPanel {
        // determine which wavesource creates thhis wave
        public WaveSource correspondWS;

        public string CorrespondingUIInfoName { get { return "PolarizedDevice"; } }

        public void ParameterChangeTrigger() {
            correspondWS?.Emit();
        }

        public void ReplaceDeviceWithType()
        {
            Track track = GetComponentInParent<Track>();
            TrackSlideInfo trackInfo = track.GetTrackSlideInfo(this);

            float prec = track.GetPrec(trackInfo);
            track.RemoveDevice(trackInfo);
            TrackSlideInfo newTrackInfo = track.AddDevice(DeviceType, prec);

            ParamPanelManager.Instance.SelectParamView(newTrackInfo.device.gameObject);
            TutorialPanel.Instance.CloseExpandPanel();
            Destroy(this);

            
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
            DeviceTypeTuple.Setter = (evt) => { 
                if (DeviceType != (DEVICETYPE)evt.newValue) {
                    DeviceType = (DEVICETYPE)evt.newValue;
                    ReplaceDeviceWithType();
                }
            };
            RotDegTuple.Setter = (evt) => { RotDeg = evt.newValue; ParameterChangeTrigger(); };
            AxisDiffDegTuple.Setter = (evt) => { AxisDiffDeg = evt.newValue; ParameterChangeTrigger(); };
        }
    }
}