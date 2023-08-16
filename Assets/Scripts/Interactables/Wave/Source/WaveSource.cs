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
        
        [Header("Debug")]
        #region PRIVATE VARIABLES
        [SerializeField] protected WaveParams _params;
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

        public void ParamNonDestructCallback() {
            WaveDisplay.RefreshDisplay();
            WaveInteract.NonDestructInteract();
        }

        public void ParamDestructCallback() {
            /*Reset Each Root WaveSource's Effective Distance*/
            WaveDisplay.RefreshDisplay();
            WaveInteract.DestructInteract();
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

            _params.EffectDistanceListener = new UnityEvent();
            _params.EffectDistanceListener.AddListener(ParamNonDestructCallback);
        }
    }
}