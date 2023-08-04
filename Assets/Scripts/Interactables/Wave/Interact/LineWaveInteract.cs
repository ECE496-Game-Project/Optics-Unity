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
        [SerializeField] private DeviceBase _hit_Device;
        [SerializeField] private WaveSource _activeWS;
        #endregion

        public void DestructInteract() {
            DebugLogger.Log(this.name, "DestructInteract");
            /*Interact Device*/
            if(_hit_Device != null)
                _hit_Device.WaveCleanup(_activeWS);

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