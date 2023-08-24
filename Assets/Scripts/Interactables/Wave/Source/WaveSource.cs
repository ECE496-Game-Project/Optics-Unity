using UnityEngine;
using UnityEngine.Events;
using CommonUtils;
using WaveUtils;
using Profiles;
using Interfaces;

namespace GO_Wave {
    public class WaveSource : MonoBehaviour {
        #region GLOBAL VARIABLES
        public I_WaveDisplay WaveDisplay;
        public I_WaveInteract WaveInteract;
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

        public virtual void ParamNonDestructCallback() {
            DebugLogger.Warning(this.name, "NonDestructCallback Not Implement yet!!!");
            //WaveDisplay.RefreshDisplay();
            //WaveInteract.NonDestructInteract();
        }

        public virtual void ParamDestructCallback() {
            WaveInteract.DestructInteract();
            WaveDisplay.RefreshDisplay();
        }

        protected void RegisterCallback() {
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

            //_params.EffectDistanceListener = new UnityEvent();
            _params.DestructableListener.AddListener(ParamDestructCallback);
        }
    }
}