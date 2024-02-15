using UnityEngine;
using Interfaces;
using CommonUtils;
using GO_Device;
using UnityEngine.Assertions;
using UnityEngine.Events;
namespace GO_Wave {
    public class LineWaveLogic : MonoBehaviour, I_WaveLogic {

        #region INSPECTOR SETTINGS
        public LayerMask InteractMask;
        #endregion

        #region PRIVRATE VARIABLES
        private DeviceBase m_hitDevice;
        private Wave m_wave;

        public UnityEvent<float> m_effDistanceChangeTrigger;
        private float m_effDistance;
        public float EffectDistance {
            get { return m_effDistance; }
            set { m_effDistance = value; m_effDistanceChangeTrigger?.Invoke(m_effDistance); }
        }
        //[TODO]: move to ParamWaveChange Class
        //private BoxCollider m_collider;
        #endregion


        public void CleanInteract() {
            if (m_hitDevice != null)
                m_hitDevice.CleanDeviceHitTrace(m_wave);
            m_hitDevice = null;
        }

        public void Interact() {
            /*Interact Device*/
            m_effDistance = m_wave.Params.RODistance;
            RaycastHit hit;

            if (
                Physics.Raycast(transform.position, transform.forward, out hit, m_effDistance, InteractMask)
                && ((1 << hit.collider.gameObject.layer) & InteractMask) != 0
            ) {
                m_hitDevice = hit.collider.gameObject.GetComponent<DeviceBase>();
                m_hitDevice.WaveHit(hit, m_wave);

                //ColliderRescale(hit.distance);
            }
        }

        //[TODO]: move to ParamWaveChange Class
        //private void ColliderRescale(float effDistance) {
        //    float scale = effDistance / 2;
        //    m_collider.center = transform.forward * scale;
        //    m_collider.size = new Vector3(1, 1, scale);
        //}

        // Called after manual awake
        public void Init(LayerMask interactMask) {
            InteractMask = interactMask;

            //[TODO]: move to ParamWaveChange Class
            //ColliderRescale(m_effDistance);
        }

        public void Awake() {
            m_wave = GetComponent<Wave>();
            if (m_wave == null) {
                DebugLogger.Error(this.name, "GameObject Doesn't contains WaveSource Script, Stop Executing.");
            }

            //m_collider = GetComponent<BoxCollider>();
            //if (m_collider == null) {
            //    DebugLogger.Error(this.name, "GameObject Doesn't contains Collider, Stop Executing.");
            //}
        }
    }
}