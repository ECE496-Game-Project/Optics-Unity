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
        [Header("DEBUG_WAVE")]
        [SerializeField] private DeviceBase m_hit_Device;
        [SerializeField] private WaveSource m_activeWS;
        #endregion

        public void CleanInteract() {
            if (m_hit_Device != null)
                m_hit_Device.CleanDeviceHitTrace(m_activeWS);
            m_hit_Device = null;
        }

        public void Interact() {
            /*Interact Device*/
            if(m_activeWS==null)Debug.Break();
            float effDistance = m_activeWS.Params.RODistance;
            RaycastHit hit;
            
            if (
                Physics.Raycast(transform.position, transform.forward, out hit, effDistance, _interactMask)
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