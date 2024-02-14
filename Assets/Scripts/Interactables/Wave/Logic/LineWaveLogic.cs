﻿using UnityEngine;
using Interfaces;
using CommonUtils;
using GO_Device;
using UnityEngine.Assertions;

namespace GO_Wave {
    public class LineWaveLogic : MonoBehaviour, I_WaveLogic {

        #region INSPECTOR SETTINGS
        [SerializeField] private LayerMask _interactMask;
        #endregion

        #region PRIVRATE VARIABLES
        private DeviceBase m_hit_Device;
        private WaveSource m_activeWS;
        private float m_effDistance;

        //[TODO]: move to ParamWaveChange Class
        private BoxCollider m_collider;
        #endregion

        public float EffectDistance { 
            get { return m_effDistance; } 
            set { m_effDistance = value; }
        }

        public void CleanInteract() {
            if (m_hit_Device != null)
                m_hit_Device.CleanDeviceHitTrace(m_activeWS);
            m_hit_Device = null;
        }

        public void Interact() {
            /*Interact Device*/
            m_effDistance = m_activeWS.Params.RODistance;
            RaycastHit hit;

            Assert.IsNotNull(transform);
            if (
                Physics.Raycast(transform.position, transform.forward, out hit, m_effDistance, _interactMask)
                && ((1 << hit.collider.gameObject.layer) & _interactMask) != 0
            ) {
                m_hit_Device = hit.collider.gameObject.GetComponent<DeviceBase>();
                m_hit_Device.WaveHit(hit, m_activeWS);

                ColliderRescale(hit.distance);
            }
        }

        //[TODO]: move to ParamWaveChange Class
        private void ColliderRescale(float effDistance) {
            float scale = effDistance / 2;
            m_collider.center = transform.forward * scale;
            m_collider.size = new Vector3(1, 1, scale);
        }

        // Called after manual awake
        public void init(I_WaveLogic srcWI) {
            this._interactMask = ((LineWaveLogic)srcWI)._interactMask;
            //[TODO]: move to ParamWaveChange Class
            ColliderRescale(m_effDistance);
        }

        public void Awake() {
            m_activeWS = GetComponent<WaveSource>();
            if (m_activeWS == null) {
                DebugLogger.Error(this.name, "GameObject Doesn't contains WaveSource Script, Stop Executing.");
            }

            m_collider = GetComponent<BoxCollider>();
            if (m_collider == null) {
                DebugLogger.Error(this.name, "GameObject Doesn't contains Collider, Stop Executing.");
            }
        }
    }
}