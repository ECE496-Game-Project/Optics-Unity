using UnityEngine;
using System;
using System.Collections.Generic;
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
        [Header("DEBUG_WAVE")]
        [SerializeField] protected WaveParams m_params;
        // Current Section's Wave Distance
        [SerializeField] protected float m_effectDistance;
        protected ParameterInfoList m_paramInfoList;
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
            if (paramName == "Name") {
                this.name = Convert.ToString(value);
                ParamChangeTrigger();
                return true;
            }
            bool res = false;
            //I_ParameterTransfer.ParameterSetHelper(m_params, paramName, value);
            if (res) ParamChangeTrigger();
            return res;
        }
        public T ParameterGet<T>(string paramName) {
            if (paramName == "Name") {
                return (T)(object)this.name;
            }
            return default;
            //I_ParameterTransfer.ParameterGetHelper<T>(m_params, paramName);
        }
        public void WaveParameterGetAll(out WAVETYPE type, out float eox, out float eoy, out float w, out float k, out float n, out float theta, out float phi) {
            type = m_params.Type;
            eox = m_params.Eox;
            eoy = m_params.Eoy;
            w = m_params.w; 
            k = m_params.k;
            n = m_params.n;
            theta = m_params.theta;
            phi = m_params.phi;
        }
        #endregion

        protected void RegisterDirCallback() {
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
        public virtual void RegisterParametersCallback(ParameterInfoList ParameterInfos) {
            // Child Wave Parameter Registration

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
            RegisterDirCallback();
        }
        
        public void Start() {
            ParamChangeTrigger();
        }
        public void UIOnClick(ParameterInfoList waveSource) {
            // DeInitalize and Initalize all Getter and Setter
        }
        //public static void UIGenerate() {
        //    //m_paramInfoList = new ParameterInfoList();
        //}
    }
}