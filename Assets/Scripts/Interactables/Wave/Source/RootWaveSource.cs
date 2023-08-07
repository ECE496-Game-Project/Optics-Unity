using UnityEngine;
using UnityEngine.Events;
using CommonUtils;
using WaveUtils;
using Profiles;
using Interfaces;

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

        public void AssignParams(WaveParams param)
        {
            param.Type = _params.Type;
            param.Eox = _params.Eox;
            param.Eoy = _params.Eoy;
            param.W = _params.W;
            param.K = _params.K;
            param.N = _params.N;
            param.Theta = _params.Theta;
            param.Phi = _params.Phi;
        }
    }
}