using UnityEngine;
using System.Reflection;
using System;

using CommonUtils;
using WaveUtils;
using Interfaces;

namespace GO_Wave {
    public class WaveSource : MonoBehaviour, I_ParameterTransfer {
        #region GLOBAL VARIABLES
        public I_WaveRender WaveDisplay;
        public I_WaveLogic WaveInteract;
        #endregion

#region PRIVATE VARIABLES
#if DEBUG_WAVE
        [Header("DEBUG_WAVE")]
        [SerializeField] protected WaveParams m_params;
        [SerializeField] protected float m_effectDistance;
#else
        protected WaveParams m_params;
        protected float m_effectDistance;
#endif
        #endregion

        #region GLOBAL METHODS
        public WaveParams Params {
            get { return m_params; }
            set {
                if (m_params != null)
                    DebugLogger.Error(this.name, "Re-Initalize WaveParameter! Break.");
                m_params = value;
            }
        }
        public float EffectDistance {
            get { return m_effectDistance; }
            set { m_effectDistance = value; }
        }
        
        public virtual void ParamChangeTrigger() {
            WaveInteract.CleanInteract();
            WaveInteract.Interact();
            WaveDisplay.RefreshDisplay();
        }

        public void DisableTrigger() {
            WaveInteract.CleanInteract();
            WaveDisplay.CleanDisplay();
        }

        public bool ParameterSet<T>(string paramName, T value) {
            if(paramName == "EffectDistance") {
                EffectDistance = (float)Convert.ToDouble(value);
                ParamChangeTrigger();
                return true;
            }
            bool res = I_ParameterTransfer.ParameterSetHelper(m_params, paramName, value);
            if(res) ParamChangeTrigger();
            return res;
        }
        public T ParameterGet<T>(string paramName) {
            if (paramName == "EffectDistance") {
                return (T)(object)EffectDistance;
            }
            return I_ParameterTransfer.ParameterGetHelper<T>(m_params, paramName);
        }
        public void ParameterGetAll(out WAVETYPE type, out float eox, out float eoy, out float w, out float k, out float n, out float theta, out float phi) {
            type = m_params.Type;
            eox = m_params.Eox;
            eoy = m_params.Eoy;
            w = m_params.W; 
            k = m_params.K;
            n = m_params.N;
            theta = m_params.Theta;
            phi = m_params.Phi;
        }
        #endregion

        private void RegisterCallback() {
            switch (m_params.Type) {
                case WAVETYPE.PLANE:
                    m_params.UHat = (in Vector3 r) => { return this.transform.right; };
                    m_params.VHat = (in Vector3 r) => { return this.transform.up; };
                    m_params.KHat = (in Vector3 r) => { return this.transform.forward; };
                    break;
                case WAVETYPE.SPHERE:
                    // [TODO][PointWave]: PointWave UVK direction Function
                    m_params.UHat = (in Vector3 r) => { return Vector3.zero; };
                    m_params.VHat = (in Vector3 r) => { return Vector3.zero; };
                    m_params.KHat = (in Vector3 r) => { return Vector3.zero; };
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Script-Generated-WaveSource Requires to Call ManualAwake.
        /// </summary>
        /// <param name="srcWP"> Pre initalized WaveParameter.</param>
        public void _awake(WaveParams srcWP) {
            WaveDisplay = GetComponent<I_WaveRender>();
            if (WaveDisplay == null)
                DebugLogger.Error(this.name, "GameObject Does not contain WaveDisplay! Stop Executing.");
            WaveInteract = GetComponent<I_WaveLogic>();
            if (WaveInteract == null)
                DebugLogger.Error(this.name, "GameObject Does not contain WaveInteract! Stop Executing.");

            /*init ActiveWaveParams*/
            m_params = srcWP;
            RegisterCallback();
        }
        
        public void Start() {
            ParamChangeTrigger();
        }
    }
}