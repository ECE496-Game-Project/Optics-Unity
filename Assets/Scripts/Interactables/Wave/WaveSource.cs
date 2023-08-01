using UnityEngine;
using UnityEngine.Events;
using CommonUtils;
using WaveUtils;
using Profiles;
using Interfaces;

namespace GO_Wave {
    public class WaveSource : MonoBehaviour, I_WaveInput {
        #region INSPECTOR SETTINGS
        public SO_WaveParams Profile;
        #endregion
        
        #region GLOBAL VARIABLES
        public I_WaveDisplay WaveDisplay;
        public I_WaveInteract WaveInteract;
        #endregion
        
        [Header("Debug")]
        #region PRIVATE VARIABLES
        [SerializeField] private WaveParams _params;
        #endregion

        #region GLOBAL METHODS
        public WaveParams Params {
            get { return _params; }
            set {
                if (_params != null)
                    DebugLogger.Error(this.name, "Re-Initalize WaveParameter! Break.");
                _params = value; 
            }
        }
        #endregion

        public void ParamNonDestructCallback() {
            WaveDisplay.RefreshDisplay();
            WaveInteract.NonDestructInteract();
        }

        public void ParamDestructCallback() {
            WaveDisplay.RefreshDisplay();
            WaveInteract.DestructInteract();
        }

        public void Rotate(Vector3 rot) {

        }

        public void Move(Vector3 dest) {
            
        }

        private void RegisterCallback() {
            switch (_params.Type) {
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

            _params.EffectDistanceListener = new UnityEvent();
            _params.EffectDistanceListener.AddListener(ParamNonDestructCallback);
        }

        /// <summary>   
        /// Only Called if Profile is Set In Inspector
        /// </summary>
        private void Awake() {
            if (Profile != null) {
                WaveDisplay = GetComponent<I_WaveDisplay>();
                if (WaveDisplay == null)
                    DebugLogger.Error(this.name, "GameObject Does not contain WaveDisplay! Stop Executing.");
                WaveInteract = GetComponent<I_WaveInteract>();
                if (WaveInteract == null)
                    DebugLogger.Error(this.name, "GameObject Does not contain WaveInteract! Stop Executing.");

                _params = new WaveParams(Profile.Parameters);
                RegisterCallback();
            }
        }

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