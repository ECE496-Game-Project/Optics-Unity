using UnityEngine;
using Interfaces;
using CommonUtils;
using GO_Device;

namespace GO_Wave {
    public class LineWaveLogic : MonoBehaviour, I_WaveLogic {

        #region INSPECTOR SETTINGS
        [SerializeField] private LayerMask _interactMask;
        #endregion

        #region PRIVRATE VARIABLES
#if DEBUG_WAVE
        [Header("DEBUG_WAVE")]
        [SerializeField] private DeviceBase m_hit_Device;
        [SerializeField] private WaveSource m_activeWS;
#else
        private DeviceBase m_hit_Device;
        private WaveSource m_activeWS;
#endif
        #endregion

        public void CleanInteract() {
            if (m_hit_Device != null)
                m_hit_Device.WaveClean(m_activeWS);
            m_hit_Device = null;
        }

        public void Interact() {
            /*Interact Device*/
            if(m_activeWS==null)Debug.Break();
            RaycastHit hit;
            if (
                Physics.Raycast(transform.position, transform.forward, out hit, m_activeWS.Params.EffectDistance.Value, _interactMask)
                && ((1 << hit.collider.gameObject.layer) & _interactMask) != 0
            ) {
                m_hit_Device = hit.collider.gameObject.GetComponent<DeviceBase>();
                m_hit_Device.WaveHit(hit, m_activeWS);
            }
        }

        public void SyncRootParam(I_WaveLogic srcWI) {
            this._interactMask = ((LineWaveLogic)srcWI)._interactMask;
        }

        public void Awake() {
            m_activeWS = GetComponent<WaveSource>();
            if (m_activeWS == null) {
                DebugLogger.Error(this.name, "GameObject Doesn't contains WaveSource Script, Stop Executing.");
            }
        }
    }
}