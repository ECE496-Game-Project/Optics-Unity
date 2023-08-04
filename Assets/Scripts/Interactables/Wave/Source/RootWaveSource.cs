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

        public override void ParamDestructCallback() {
            _params.EffectDistance = _profile.Parameters.EffectDistance;
            
            base.ParamDestructCallback();
        }

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
    }
}