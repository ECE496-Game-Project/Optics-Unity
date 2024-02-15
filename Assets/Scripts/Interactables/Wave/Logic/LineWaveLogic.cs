using UnityEngine;
using Interfaces;
using CommonUtils;
using GO_Device;
using UnityEngine.Assertions;
using UnityEngine.Events;
namespace GO_Wave {
    public class LineWaveLogic : MonoBehaviour, I_WaveLogic {
        #region PRIVRATE VARIABLES
        private DeviceBase m_hitDevice;
        private BoxCollider m_collider;
        private Wave m_wave;
        private LayerMask m_interactMask;
        private float m_effDistance;
        #endregion

        #region PUBLIC VARIABLES
        [HideInInspector] public UnityEvent<float> m_effDistanceChangeTrigger;
        public float EffectDistance {
            get { return m_effDistance; }
            set { m_effDistance = value; m_effDistanceChangeTrigger?.Invoke(m_effDistance); }
        }
        public LayerMask InteractMask { get { return m_interactMask; } }
        #endregion

        #region GLOBAL METHOD
        public void CleanInteract() {
            if (m_hitDevice != null)
                m_hitDevice.CleanDeviceHitTrace(m_wave);
            m_hitDevice = null;
        }

        public void Interact() {
            m_effDistance = m_wave.Params.RODistance;
            RaycastHit hit;

            if (
                Physics.Raycast(transform.position, transform.forward, out hit, m_effDistance, m_interactMask)
                && ((1 << hit.collider.gameObject.layer) & m_interactMask) != 0
            ) {
                m_hitDevice = hit.collider.gameObject.GetComponent<DeviceBase>();
                m_hitDevice.WaveHit(hit, m_wave);

                ColliderRescale(hit.distance);
            }
        }

        public void Init(LayerMask interactMask) {
            m_interactMask = interactMask;

            m_wave = GetComponent<Wave>();
            if (m_wave == null)
                DebugLogger.Error(this.name, "GameObject Doesn't contains Wave Script, Stop Executing.");
            m_effDistance = m_wave.Params.RODistance;

            m_collider = GetComponent<BoxCollider>();
            if (m_collider == null)
                DebugLogger.Error(this.name, "GameObject Doesn't contains BoxCollider, Stop Executing.");
            m_collider.isTrigger = true;
            ColliderRescale(m_effDistance);
        }
        #endregion

        #region HELPER METHOD
        private void ColliderRescale(float effDistance) {
            float scale = effDistance / 2;
            // offset Collider to not fully cover wave
            float offset = 1f;
            m_collider.center = transform.forward * scale;
            m_collider.size = new Vector3(1, 1, effDistance - offset);
        }
        #endregion
    }
}