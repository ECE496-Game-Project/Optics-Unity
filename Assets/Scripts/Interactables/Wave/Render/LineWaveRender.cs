using System.Collections.Generic;
using UnityEngine;
using CommonUtils;
using WaveUtils;
using Interfaces;
using ObjectPool;

namespace GO_Wave {
    public class LineWaveRender : MonoBehaviour, I_WaveRender {

        #region INSPECTOR SETTINGS
        [Header("Line Wave Display Settings")]
        [SerializeField] private float _perSampleSpaceLength;
        [SerializeField] private GameObject _samplePointPrefab;
        
        [Header("Timescale Display Settings")]
        // 感觉这样控制效果不太好,需要找其他方法
        [Range(0.0f, 5.0f)]
        [SerializeField] private float _timeScale = 1.0f;
        #endregion

        #region PRIVRATE VARIABLES
        [Header("DEBUG_WAVE")]
        [SerializeField] private int m_SampleCount;
        [SerializeField] private Stack<LineWaveSample> _samplePointList;
        [SerializeField] private WaveSource _activeWS;
        #endregion

        #region GLOBAL METHOD
        public void CleanDisplay() {
            while (_samplePointList.Count > 0) {
                var popitem = _samplePointList.Pop();
                popitem.transform.parent = LineWaveSamplePool.Instance.transform;
                LineWaveSamplePool.Instance.Pool.Release(popitem);
            }
            m_SampleCount = 0;
        }
        public void RefreshDisplay() {
            /*Reposition All Sample Points base on WaveSource*/
            m_SampleCount = Mathf.FloorToInt(_activeWS.EffectDistance / _perSampleSpaceLength);
            
            int diff = m_SampleCount - _samplePointList.Count;
            while (diff > 0) {
                LineWaveSample sample = LineWaveSamplePool.Instance.Pool.Get();
                sample.transform.rotation = Quaternion.LookRotation(this.transform.forward, this.transform.up);
                sample.transform.parent = this.transform;
                _samplePointList.Push(sample);
                diff--;
            }

            if (diff == 0) goto RePosEnd;

            while(diff < 0) {
                var popitem = _samplePointList.Pop();
                popitem.transform.parent = LineWaveSamplePool.Instance.transform;
                LineWaveSamplePool.Instance.Pool.Release(popitem);
                diff++;
            }

            RePosEnd:
            int i = 0;
            foreach(var item in _samplePointList) {
                i++;
                item.transform.position = this.transform.position + i * _perSampleSpaceLength * this.transform.forward;
            }
        }

        public void UpdateDisplay() {
            foreach (var item in _samplePointList) {
                Vector3 vec = WaveAlgorithm.CalcIrradiance(
                    item.transform.position - this.transform.position,
                    Time.time * _timeScale,
                    _activeWS.Params
                );
                item.UpdateEVec(vec);
            }
        }

        public void SyncRootParam(I_WaveRender rootWD) {
            this._perSampleSpaceLength = ((LineWaveRender)rootWD)._perSampleSpaceLength;
            this._samplePointPrefab = ((LineWaveRender)rootWD)._samplePointPrefab;
            this._timeScale = ((LineWaveRender)rootWD)._timeScale;
        }
        #endregion

        private void Awake() {
            m_SampleCount = 0;
            _samplePointList = new Stack<LineWaveSample>();

            _activeWS = this.transform.GetComponent<WaveSource>();
            if (_activeWS == null) {
                DebugLogger.Error(this.name, "GameObject Doesn't contains WaveSource Script, Stop Executing.");
            }
        }

        public void Start() {
            if (_samplePointPrefab == null || _samplePointPrefab.GetComponent<LineWaveSample>() == null) {
                DebugLogger.Error(this.name, "Prefab does not contains WaveLineSample Script! Stop Executing.");
            }
        }

        private void Update() {
            UpdateDisplay();
        }
    }
}