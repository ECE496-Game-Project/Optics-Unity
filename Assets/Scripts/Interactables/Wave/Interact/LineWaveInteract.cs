using UnityEngine;
using Interfaces;
using CommonUtils;
using GO_Device;

namespace GO_Wave {
    public class LineWaveInteract : MonoBehaviour, I_WaveInteract {

        #region INSPECTOR SETTINGS
        [SerializeField] private LayerMask _interactMask;
        #endregion

        #region PRIVRATE VARIABLES
#if DEBUG_WAVE
        [Header("DEBUG_WAVE")]
        [SerializeField] private DeviceBase _hit_Device;
        [SerializeField] private WaveSource _activeWS;
#else
        private DeviceBase _hit_Device;
        private WaveSource _activeWS;
#endif
        #endregion

        public void CleanInteract() {
            if (_hit_Device != null)
                _hit_Device.WaveClean(_activeWS);
            _hit_Device = null;
        }

        public void DestructInteract() {
            /*Interact Device*/
            CleanInteract();
            if(_activeWS==null)Debug.Break();
            RaycastHit hit;
            if (
                Physics.Raycast(transform.position, transform.forward, out hit, _activeWS.Params.EffectDistance, _interactMask)
                && ((1 << hit.collider.gameObject.layer) & _interactMask) != 0
            ) {
                _hit_Device = hit.collider.gameObject.GetComponent<DeviceBase>();
                _hit_Device.WaveHit(hit, _activeWS);
            }
        }

        public void NonDestructInteract() {

        }

        public void SyncRootParam(I_WaveInteract srcWI) {
            this._interactMask = ((LineWaveInteract)srcWI)._interactMask;
        }

        public void Awake() {
            _activeWS = GetComponent<WaveSource>();
            if (_activeWS == null) {
                DebugLogger.Error(this.name, "GameObject Doesn't contains WaveSource Script, Stop Executing.");
            }
        }
    }
}