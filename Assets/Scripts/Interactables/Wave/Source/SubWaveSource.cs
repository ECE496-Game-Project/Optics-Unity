using UnityEngine;
using UnityEngine.Events;
using CommonUtils;
using WaveUtils;
using Profiles;
using Interfaces;

namespace GO_Wave {
    public class SubWaveSource : WaveSource {     
        /// <summary>
        /// Script-Generated-WaveSource Requires to Call Prepare, since it does not have Profile Invoke.
        /// </summary>
        /// <param name="srcWP"> Pre initalized WaveParameter.</param>
        public void Prepare(WaveParams srcWP) {
            WaveDisplay = GetComponent<I_WaveDisplay>();
            if (WaveDisplay == null)
                DebugLogger.Error(this.name, "GameObject Does not contain WaveDisplay! Stop Executing.");
            WaveInteract = GetComponent<I_WaveInteract>();
            if (WaveInteract == null)
                DebugLogger.Error(this.name, "GameObject Does not contain WaveInteract! Stop Executing.");

            /*init ActiveWaveParams*/
            _params = srcWP;
            RegisterCallback();
        }
    }
}