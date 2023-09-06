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
        [SerializeField] private float _perSampleSpaceLength;
        [SerializeField] private GameObject _samplePointPrefab;
        
        [Header("Timescale Display Settings")]
        // 感觉这样控制效果不太好,需要找其他方法
        [Range(0.0f, 5.0f)]
        [SerializeField] private float _timeScale = 1.0f;
        #endregion

        #region PRIVRATE VARIABLES
        private bool _isPause = false;
#if DEBUG_WAVE
        [Header("DEBUG_WAVE")]
        [SerializeField] private int m_SampleCount;
        [SerializeField] private List<LineWaveSample> _samplePointList;
        [SerializeField] private WaveSource _activeWS;
#else
        private int m_SampleCount;
        private List<LineWaveSample> _samplePointList;
        private WaveSource _activeWS;
#endif
        #endregion

        #region GLOBAL METHOD
        public void CleanDisplay() {
            foreach (LineWaveSample sample in _samplePointList) {
                LineWaveSamplePool.Instance.Pool.Release(sample);
                //sample.DisableDisplay();
            }
            _samplePointList.Clear();
        }
        public void RefreshDisplay() {
            _isPause = false;

            /*Reposition All Sample Points base on WaveSource*/
            m_SampleCount = Mathf.FloorToInt(_activeWS.Params.EffectDistance / _perSampleSpaceLength);

            int diff = m_SampleCount - _samplePointList.Count;
            while(diff > 0) {
                LineWaveSample sample = LineWaveSamplePool.Instance.Pool.Get();
                sample.transform.rotation = Quaternion.LookRotation(this.transform.forward, this.transform.up);
                sample.transform.parent = this.transform;
                _samplePointList.Add(sample);
                diff--;
            }

            if (diff == 0) goto RePosEnd;

            while(diff < 0) {
                _samplePointList[_samplePointList.Count + diff].transform.parent = LineWaveSamplePool.Instance.transform;
                LineWaveSamplePool.Instance.Pool.Release(_samplePointList[_samplePointList.Count + diff]);
                _samplePointList.RemoveAt(_samplePointList.Count + diff);
                diff++;
            }

            RePosEnd:
            for (int i = 0; i < m_SampleCount; i++) {
                _samplePointList[i].name = _samplePointPrefab.name + "[" + i + "]";
                _samplePointList[i].transform.position = this.transform.position + i * _perSampleSpaceLength * this.transform.forward;
            }
        }

        public void UpdateDisplay() {
            for (int i = 0; i < m_SampleCount; i++) {
                Vector3 vec = WaveAlgorithm.CalcIrradiance(_samplePointList[i].transform.position - this.transform.position, Time.time * _timeScale, _activeWS.Params);
                _samplePointList[i].UpdateEVec(vec);
            }
        }

        public void SyncRootParam(I_WaveDisplay rootWD) {
            this._perSampleSpaceLength = ((LineWaveDisplay)rootWD)._perSampleSpaceLength;
            this._samplePointPrefab = ((LineWaveDisplay)rootWD)._samplePointPrefab;
            this._timeScale = ((LineWaveDisplay)rootWD)._timeScale;
        }
        #endregion

        private void Awake() {
            m_SampleCount = 0;
            _samplePointList = new List<LineWaveSample>();

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
            if(!_isPause) UpdateDisplay();
        }
    }
}