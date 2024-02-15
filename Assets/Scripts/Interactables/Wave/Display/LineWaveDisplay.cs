using System.Collections.Generic;
using UnityEngine;
using CommonUtils;
using WaveUtils;
using Interfaces;
using ObjectPool;

namespace GO_Wave {
    public class LineWaveDisplay : MonoBehaviour, I_WaveDisplay {

        #region INSPECTOR SETTINGS
        [Header("Line Wave Display Settings")]
        public float SampleResolution;
        #endregion

        #region PRIVRATE VARIABLES
        [Header("DEBUG_WAVE")]
        [SerializeField] private Stack<LineWaveSample> m_samplePointList;
        [SerializeField] private Wave _activeWS;
        #endregion

        #region GLOBAL METHOD
        public void CleanDisplay() {
            while (m_samplePointList.Count > 0) {
                var popitem = m_samplePointList.Pop();
                popitem.transform.parent = LineWaveSamplePool.Instance.transform;
                LineWaveSamplePool.Instance.Pool.Release(popitem);
            }
        }
        public void RefreshDisplay() {
            /*Reposition All Sample Points base on WaveSource*/
            int sampleCount = Mathf.FloorToInt(((LineWaveLogic)_activeWS.WaveLogic).EffectDistance / SampleResolution);
            
            int diff = sampleCount - m_samplePointList.Count;
            while (diff > 0) {
                LineWaveSample sample = LineWaveSamplePool.Instance.Pool.Get();
                
                sample.transform.rotation = Quaternion.LookRotation(this.transform.forward, this.transform.up);
                sample.transform.parent = this.transform;
                m_samplePointList.Push(sample);
                diff--;
            }

            if (diff == 0) goto RePosEnd;

            while(diff < 0) {
                var popitem = m_samplePointList.Pop();
                popitem.transform.parent = LineWaveSamplePool.Instance.transform;
                LineWaveSamplePool.Instance.Pool.Release(popitem);
                diff++;
            }

            RePosEnd:
            int i = 0;
            foreach(var item in m_samplePointList) {
                i++;
                item.transform.position = this.transform.position + i * SampleResolution * this.transform.forward;
            }
        }

        public void UpdateDisplay() {
            foreach (var item in m_samplePointList) {
                Vector3 vec = WaveAlgorithm.CalcIrradiance(
                    item.transform.position - this.transform.position,
                    Time.time,
                    _activeWS.Params
                );
                item.UpdateEVec(vec);
            }
        }

        public void Init(float sampleResolution) {
            this.SampleResolution = sampleResolution;
        }
        #endregion

        private void Awake() {
            m_samplePointList = new Stack<LineWaveSample>();

            _activeWS = this.transform.GetComponent<Wave>();
            if (_activeWS == null) {
                DebugLogger.Error(this.name, "GameObject Doesn't contains WaveSource Script, Stop Executing.");
            }
        }
        private void Update() {
            UpdateDisplay();
        }
    }
}