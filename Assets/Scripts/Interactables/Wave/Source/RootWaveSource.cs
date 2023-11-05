using UnityEngine;
using CommonUtils;
using WaveUtils;
using Profiles;

namespace GO_Wave {
    public class RootWaveSource : WaveSource {
        #region INSPECTOR SETTINGS
        [SerializeField] private SO_WaveParams _profile;
        #endregion

        /// <summary>   
        /// Only Called if Profile is Set In Inspector
        /// </summary>
        private void Awake() {
            if(_profile == null)
                DebugLogger.Error(this.name, "RootWave Does not contain WaveProfile! Stop Executing.");

            _awake(new WaveParams(_profile.Parameters));
        }

        /// <summary>
        /// Need Manual Reset Effective Distance since Distance Modified during Interaction.
        /// </summary>
        public override void ParamChangeTrigger() {
            EffectDistance = _profile.Parameters.RODistance;
            base.ParamChangeTrigger();
        }
    }
}