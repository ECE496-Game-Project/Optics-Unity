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
        [SerializeField] protected WaveParams _params;
#else
        protected WaveParams _params;
#endif
        #endregion

        #region GLOBAL METHODS
        public WaveParams Params
        {
            get { return _params; }
            set
            {
                if (_params != null)
                    DebugLogger.Error(this.name, "Re-Initalize WaveParameter! Break.");
                _params = value;
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
            switch (_params.Type.Value) {
                case WAVETYPE.PARALLEL:
                    _params.UHat = (in Vector3 r) => { return this.transform.right; };
                    _params.VHat = (in Vector3 r) => { return this.transform.up; };
                    _params.KHat = (in Vector3 r) => { return this.transform.forward; };
                    break;
                case WAVETYPE.POINT:
                    // [TODO][PointWave]: PointWave UVK direction Function
                    _params.UHat = (in Vector3 r) => { return Vector3.zero; };
                    _params.VHat = (in Vector3 r) => { return Vector3.zero; };
                    _params.KHat = (in Vector3 r) => { return Vector3.zero; };
                    break;
                default:
                    break;
            }

            //_params.VTOMListener.AddListener(ParamChangeTrigger);
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
            _params = srcWP;
            RegisterCallback();
        }
        public void Start() {
            ParamChangeTrigger();
        }
    }
}