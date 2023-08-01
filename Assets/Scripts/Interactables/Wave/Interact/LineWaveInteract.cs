using UnityEngine;
using System.Collections.Generic;
using Interfaces;
using CommonUtils;
using GO_Device;

namespace GO_Wave {
    public class LineWaveInteract : MonoBehaviour, I_WaveInteract {

        #region INSPECTOR SETTINGS
        [SerializeField] private LayerMask _interactMask;
        #endregion

        [Header("Debug")]
        #region PRIVRATE VARIABLES
        [SerializeField] private Dictionary<DeviceBase, WaveSource> _childWaves_Device;
        [SerializeField] private WaveSource _activeWS;
        #endregion

        public void DestructInteract() {
            Debug.Log("DestructInteract");
            /*Interact Device*/
            foreach (var childWave in _childWaves_Device) {
                Destroy(childWave.Value.gameObject);
            }
            _childWaves_Device.Clear();

           RaycastHit hit;
            if (
                Physics.Raycast(transform.position, transform.forward, out hit, _activeWS.Params.EffectDistance, _interactMask)
                && ((1 << hit.collider.gameObject.layer) & _interactMask) != 0
            ) {
                hit.collider.gameObject.GetComponent<DeviceBase>().WaveHit(hit, _activeWS);
            }
        }

        public void NonDestructInteract() {

        }

        private void Awake() {
            _childWaves_Device = new Dictionary<DeviceBase, WaveSource>();
        }

        public void Prepare(I_WaveInteract srcWI) {
            this._interactMask = ((LineWaveInteract)srcWI)._interactMask;
        }

        public void Start() {
            _activeWS = GetComponent<WaveSource>();
            if (_activeWS == null) {
                DebugLogger.Error(this.name, "GameObject Doesn't contains WaveSource Script, Stop Executing.");
            }

            DestructInteract();
        }
    }
}