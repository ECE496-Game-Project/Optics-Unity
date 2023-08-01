using System.Collections.Generic;
using UnityEngine;
using CommonUtils;
using WaveUtils;
using Interfaces;

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

        [Header("Debug")]
        #region PRIVRATE VARIABLES
        [SerializeField] private int m_SampleCount;
        [SerializeField] private List<LineWaveSample> _samplePointList;
        [SerializeField] private WaveSource _activeWS;
        #endregion


        #region GLOBAL METHOD
        public void RefreshDisplay() {
            /*Reposition All Sample Points base on WaveSource*/
            m_SampleCount = Mathf.FloorToInt(_activeWS.Params.EffectDistance / _perSampleSpaceLength);

            int diff = m_SampleCount - _samplePointList.Count;
            while(diff > 0) {
                _samplePointList.Add(
                    Object.Instantiate(
                        _samplePointPrefab,
                        this.transform.position,
                        Quaternion.LookRotation(this.transform.forward, this.transform.up),
                        this.transform
                    ).GetComponent<LineWaveSample>()
                );
                diff--;
            }

            if (diff == 0) goto RePosEnd;

            while(diff < 0) {
                _samplePointList[_samplePointList.Count + diff].DisableDisplay();
                diff++;
            }

            RePosEnd:
            for (int i = 0; i < m_SampleCount; i++) {
                _samplePointList[i].name = _samplePointPrefab.name + "[" + i + "]";
                _samplePointList[i].transform.position = this.transform.position + i * _perSampleSpaceLength * this.transform.forward;
                _samplePointList[i].EnableDisplay();
            }
        }

        public void UpdateDisplay() {
            for (int i = 0; i < m_SampleCount; i++) {
                Vector3 vec = WaveAlgorithm.CalcIrradiance(_samplePointList[i].transform.position - this.transform.position, Time.time * _timeScale, _activeWS.Params);
                _samplePointList[i].UpdateEVec(vec);
            }
        }

        
        #endregion

        private void Awake() {
            m_SampleCount = 0;
            _samplePointList = new List<LineWaveSample>();
        }
        public void Prepare(I_WaveDisplay srcWD) {
            this._perSampleSpaceLength = ((LineWaveDisplay)srcWD)._perSampleSpaceLength;
            this._samplePointPrefab = ((LineWaveDisplay)srcWD)._samplePointPrefab;
            this._timeScale = ((LineWaveDisplay)srcWD)._timeScale;
        }

        public void Start() {
            _activeWS = this.transform.GetComponent<WaveSource>();
            if (_activeWS == null) {
                DebugLogger.Error(this.name, "GameObject Doesn't contains WaveSource Script, Stop Executing.");
            }

            if (_samplePointPrefab == null || _samplePointPrefab.GetComponent<LineWaveSample>() == null) {
                DebugLogger.Error(this.name, "Prefab does not contains WaveLineSample Script! Stop Executing.");
            }

            RefreshDisplay();
        }

        private void Update() {
            UpdateDisplay();
        }
    }
}