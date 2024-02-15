using System.Collections.Generic;
using UnityEngine;
using CommonUtils;
using WaveUtils;
using Interfaces;
using ObjectPool;

namespace GO_Wave {
    public class LineWaveDisplay : MonoBehaviour, I_WaveDisplay {
        #region PRIVRATE VARIABLES
        private float m_sampleResolution;
        public float SampleResolution { get { return m_sampleResolution; } }
        private Stack<LineWaveSample> m_samplePointList;
        private Wave m_wave;
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
            int sampleCount = Mathf.FloorToInt(((LineWaveLogic)m_wave.WaveLogic).EffectDistance / m_sampleResolution);
            
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
                item.transform.position = this.transform.position + i * m_sampleResolution * this.transform.forward;
            }
        }
        public void Init(float sampleResolution) {
            this.m_sampleResolution = sampleResolution;

            m_wave = GetComponent<Wave>();
            if (m_wave == null) {
                DebugLogger.Error(this.name, "GameObject Doesn't contains WaveSource Script, Stop Executing.");
            }

            m_samplePointList = new Stack<LineWaveSample>();
        }
        #endregion

        private void Update() {
            foreach (var item in m_samplePointList) {
                Vector3 vec = WaveAlgorithm.CalcIrradiance(
                    item.transform.position,
                    Time.time,
                    m_wave.Params
                );
                item.UpdateEVec(vec);
            }
        }
    }
}