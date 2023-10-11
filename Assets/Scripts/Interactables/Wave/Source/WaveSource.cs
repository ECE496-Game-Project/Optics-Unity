using UnityEngine;
using UnityEngine.Events;
using CommonUtils;
using WaveUtils;
using Profiles;
using Interfaces;

namespace GO_Wave {
    public class WaveSource : MonoBehaviour {
        #region GLOBAL VARIABLES
        public I_WaveRender WaveDisplay;
        public I_WaveLogic WaveInteract;
        #endregion

        #region PRIVATE VARIABLES
#if DEBUG_WAVE
        [Header("DEBUG_WAVE")]
        [SerializeField] protected WaveParams m_params;
#else
        protected WaveParams m_params;
#endif
        #endregion

        #region GLOBAL METHODS
        public WaveParams Params
        {
            get { return m_params; }
            set
            {
                if (m_params != null)
                    DebugLogger.Error(this.name, "Re-Initalize WaveParameter! Break.");
                m_params = value;
            }
        }
#endregion
        public virtual void ParamChangeTrigger() {
            WaveInteract.CleanInteract();
            WaveInteract.Interact();
            WaveDisplay.RefreshDisplay();
        }

        public void DisableTrigger() {
            WaveInteract.CleanInteract();
            WaveDisplay.CleanDisplay();
        }

        private void RegisterCallback() {
            switch (m_params.Type) {
                case WAVETYPE.PARALLEL:
                    m_params.UHat = (in Vector3 r) => { return this.transform.right; };
                    m_params.VHat = (in Vector3 r) => { return this.transform.up; };
                    m_params.KHat = (in Vector3 r) => { return this.transform.forward; };
                    break;
                case WAVETYPE.POINT:
                    // [TODO][PointWave]: PointWave UVK direction Function
                    m_params.UHat = (in Vector3 r) => { return Vector3.zero; };
                    m_params.VHat = (in Vector3 r) => { return Vector3.zero; };
                    m_params.KHat = (in Vector3 r) => { return Vector3.zero; };
                    break;
                default:
                    break;
            }

            //m_params.Eox.m_logicEvent.AddListener(ParamChangeTrigger);
        }

        /// <summary>
        /// Script-Generated-WaveSource Requires to Call ManualAwake.
        /// </summary>
        /// <param name="srcWP"> Pre initalized WaveParameter.</param>
        public void _awake(WaveParams srcWP) {
            WaveDisplay = GetComponent<I_WaveRender>();
            if (WaveDisplay == null)
                DebugLogger.Error(this.name, "GameObject Does not contain WaveDisplay! Stop Executing.");
            WaveInteract = GetComponent<I_WaveLogic>();
            if (WaveInteract == null)
                DebugLogger.Error(this.name, "GameObject Does not contain WaveInteract! Stop Executing.");

            /*init ActiveWaveParams*/
            m_params = srcWP;
            RegisterCallback();
        }
        public void Start() {
            ParamChangeTrigger();
        }
    }
}