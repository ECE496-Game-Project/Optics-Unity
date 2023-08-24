using UnityEngine;
using System.Runtime.InteropServices;
using WaveUtils;
using CommonUtils;
using GO_Wave;

public class WaveSourcePresenter : MonoBehaviour
{
    [SerializeField] private WaveSource _activeWS;

    [DllImport("__Internal")]
    private static extern void ReceiveParams(float input, int idx);

    [DllImport("__Internal")]
    private static extern void ReceiveWaveType(int input);

    // Use this for initialization
    void Start()
	{
        _activeWS = GetComponent<WaveSource>();
        if (_activeWS == null)
        {
            DebugLogger.Error(this.name, "GameObject Doesn't contains WaveSource Script, Stop Executing.");
        }
        else
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            SendParamsToWeb();
#endif
        }
    }

    public void SendParamsToWeb()
    {
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

    public void SetTypeFromWeb(string value)
    {
        if (int.TryParse(value, out int val))
        {
            if (val == (int)WAVETYPE.INVALID) _activeWS.Params.Type = WAVETYPE.INVALID;
            if (val == (int)WAVETYPE.PARALLEL) _activeWS.Params.Type = WAVETYPE.PARALLEL;
            if (val == (int)WAVETYPE.POINT) _activeWS.Params.Type = WAVETYPE.POINT;
        }

    }

    public void SetEoxFromWeb(string value)
    {
        if (float.TryParse(value, out float val))
            _activeWS.Params.Eox = val;
    }

    public void SetEoyFromWeb(string value)
    {
        if (float.TryParse(value, out float val))
            _activeWS.Params.Eoy = val;
    }

    public void SetWFromWeb(string value)
    {
        if (float.TryParse(value, out float val))
            _activeWS.Params.W = val;
    }

    public void SetKFromWeb(string value)
    {
        if (float.TryParse(value, out float val))
            _activeWS.Params.K = val;
    }

    public void SetNFromWeb(string value)
    {
        if (float.TryParse(value, out float val))
            _activeWS.Params.N = val;
    }

    public void SetThetaFromWeb(string value)
    {
        if (float.TryParse(value, out float val))
            _activeWS.Params.Theta = val;
    }

    public void SetPhiFromWeb(string value)
    {
        if (float.TryParse(value, out float val))
            _activeWS.Params.Phi = val;
    }

    public void SetDistanceFromWeb(string value)
    {
        if (float.TryParse(value, out float val))
            _activeWS.Params.EffectDistance = val;
    }
}

