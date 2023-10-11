﻿using UnityEngine;
using System.Runtime.InteropServices;
using WaveUtils;
using CommonUtils;
using GO_Wave;

public class WaveSourcePresenter : MonoBehaviour {
    [SerializeField] private WaveSource _activeWS;

    [DllImport("__Internal")]
    private static extern void ReceiveParams(float input, int idx);

    [DllImport("__Internal")]
    private static extern void ReceiveWaveType(int input);

    // Use this for initialization
    void Start() {
        _activeWS = GetComponent<WaveSource>();
        if (_activeWS == null) {
            DebugLogger.Error(this.name, "GameObject Doesn't contains WaveSource Script, Stop Executing.");
        }
        else {
#if !UNITY_EDITOR && UNITY_WEBGL
            SendParamsToWeb();
#endif
        }
    }

    public void SendParamsToWeb() {
        ReceiveWaveType((int)_activeWS.Params.Type);
        ReceiveParams(_activeWS.Params.Eox, 0);
        ReceiveParams(_activeWS.Params.Eoy, 1);
        ReceiveParams(_activeWS.Params.W, 2);
        ReceiveParams(_activeWS.Params.K, 3);
        ReceiveParams(_activeWS.Params.N, 4);
        ReceiveParams(_activeWS.Params.Theta, 5);
        ReceiveParams(_activeWS.Params.Phi, 6);
        ReceiveParams(_activeWS.Params.EffectDistance, 7);
    }

    // TODO: Add model change event to affect view
    // View KHat, VHat, UHat as Vector3, each element is a float. No Set

    public void SetTypeFromWeb(string value)
    {
        if (int.TryParse(value, out int val))
        {
            if (val == (int)WAVETYPE.INVALID) _activeWS.Params.Type = WAVETYPE.INVALID;
            if (val == (int)WAVETYPE.PARALLEL) _activeWS.Params.Type = WAVETYPE.PARALLEL;
            if (val == (int)WAVETYPE.POINT) _activeWS.Params.Type = WAVETYPE.POINT;
        }
    }

    public void SetEoxFromWeb(string value) {
        if (float.TryParse(value, out float val)) {
            //_activeWS.Params.Eox.WEValue = val;
        }
    }

    public void SetEoyFromWeb(string value) {
        if (float.TryParse(value, out float val)) {
            //_activeWS.Params.Eoy.WEValue = val;
        }
    }

    public void SetWFromWeb(string value) {
        if (float.TryParse(value, out float val)) {
            //_activeWS.Params.W.WEValue = val;
        }
    }

    public void SetKFromWeb(string value) {
        if (float.TryParse(value, out float val)) {
            //_activeWS.Params.K.WEValue = val;
        }
    }

    public void SetNFromWeb(string value) {
        if (float.TryParse(value, out float val)) {
            //_activeWS.Params.N.WEValue = val;
        }
    }

    public void SetThetaFromWeb(string value) {
        if (float.TryParse(value, out float val)) {
            //_activeWS.Params.Theta.WEValue = val;
        }
    }

    public void SetPhiFromWeb(string value) {
        if (float.TryParse(value, out float val)) {
            //_activeWS.Params.Phi.WEValue = val;
        }
    }

    public void SetDistanceFromWeb(string value) {
        if (float.TryParse(value, out float val)) {
            //_activeWS.Params.EffectDistance.WEValue = val;
        }
    }

    public void SetParams(string value, int idx)
    {

    }
}

// presenter -> view
// 根据data，辨别view type

// traverse:
// 1. Model-> Presenter: 生成list，用于SetValue to Model
// 2. C# Presenter -> JS Prresenter: 生成JSON, 用于生成view的layout
// 3. JS Presenter: 生成Dictionary based on JSON，用于Model到View的设置

// 1. Model -> C# Prensenter: Subscribe Event to listen value change
// 2. C# Presenter -> JS Presenter: Directly Set Value thourgh callback

// 1. View -> JS Prresenter -> C# Presenter: AddEventListener
// 2. C# Presenter -> Model: Call delegate in SetParams

// presenter -> model
// List<Action<string>>
// data param name, type, 特殊要求比如[range],
// according to name, add action into
// 每次新的view，重新生成List
// Store SetValue的delegate into this list


// data
/*
public class Param<T> where T : IConvertible
{
    T m_value;
    T Value { get; set; }

    // one for UI one for unity
    //UnityEvent e1, e2;

    public void SetValue(string str)
    {
        List<Param<float>> hat = new List<Param<float>>();
        m_value = (T)Convert.ChangeType(str, typeof(T));
    }
}
*/