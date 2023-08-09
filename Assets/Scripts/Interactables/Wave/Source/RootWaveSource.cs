using UnityEngine;
using UnityEngine.Events;
using CommonUtils;
using WaveUtils;
using Profiles;
using Interfaces;
using System.Runtime.InteropServices;

namespace GO_Wave {
    public class RootWaveSource : WaveSource {
        #region INSPECTOR SETTINGS
        [SerializeField] private SO_WaveParams _profile;
        #endregion

        /// <summary>   
        /// Only Called if Profile is Set In Inspector
        /// </summary>
        private void Awake() {
            if (_profile != null) {
                WaveDisplay = GetComponent<I_WaveDisplay>();
                if (WaveDisplay == null)
                    DebugLogger.Error(this.name, "GameObject Does not contain WaveDisplay! Stop Executing.");
                WaveInteract = GetComponent<I_WaveInteract>();
                if (WaveInteract == null)
                    DebugLogger.Error(this.name, "GameObject Does not contain WaveInteract! Stop Executing.");

                _params = new WaveParams(_profile.Parameters);
                RegisterCallback();
            }
        }

        public void Start()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            SendParamsToWeb();
#endif
        }

        [DllImport("__Internal")]
        private static extern void ReceiveParams(float input, int idx);

        [DllImport("__Internal")]
        private static extern void ReceiveWaveType(int input);

        public void SendParamsToWeb()
        {
            ReceiveWaveType((int)_params.Type);
            ReceiveParams(_params.Eox, 0);
            ReceiveParams(_params.Eoy, 1);
            ReceiveParams(_params.W, 2);
            ReceiveParams(_params.K, 3);
            ReceiveParams(_params.N, 4);
            ReceiveParams(_params.Theta, 5);
            ReceiveParams(_params.Phi, 6);
        }
    }
}