using System.Collections.Generic;
using UnityEngine;
using CommonUtils;
using WaveUtils;

namespace GO_Wave {
    public class WaveLineDisplay : MonoBehaviour {
        [Header("Line Wave Display Settings")]
        [SerializeField] private float _perSampleSpaceLength;
        [SerializeField] private GameObject _samplePointPrefab;
        
        private int m_SampleCount = 0;
        private List<WaveLineSample> _samplePointList;

        [Header("Timescale Display Settings")]
        // [TODO]: 感觉这样控制效果不太好,需要找其他方法
        [Range(0.0f, 5.0f)]
        [SerializeField] private float _timeScale = 1.0f;

        private WaveSource _activeWS;

        private void RePositionSamples() {
            m_SampleCount = Mathf.FloorToInt(_activeWS.EffectDistance / _perSampleSpaceLength);

            int diff = m_SampleCount - _samplePointList.Count;
            while(diff > 0) {
                _samplePointList.Add(
                    Object.Instantiate(
                        _samplePointPrefab,
                        this.transform.position,
                        Quaternion.LookRotation(this.transform.forward, this.transform.up),
                        this.transform
                    ).GetComponent<WaveLineSample>()
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
                _samplePointList[i].transform.position = this.transform.position + i * _perSampleSpaceLength * this.transform.forward;
                _samplePointList[i].EnableDisplay();
            }
        }


        private void Start() {
            _activeWS = GetComponent<WaveSource>();
            if (_activeWS == null) {
                DebugLogger.Error(this.name, "GameObject Doesn't contains WaveSource Script, Stop Executing.");
            }
            if (_samplePointPrefab.GetComponent<WaveLineSample>() == null) {
                DebugLogger.Error(this.name, "Prefab Doesn't contains WaveLineSample Script, Stop Executing.");
            }

            _samplePointList = new List<WaveLineSample>();

            RePositionSamples();
        }

        private void Update() {
            for (int i = 0; i < m_SampleCount; i++) {
                Vector3 vec = WaveAlgorithm.CalcIrradiance(_samplePointList[i].transform.position - this.transform.position, Time.time * _timeScale, _activeWS.ActiveWaveParams);

                _samplePointList[i].UpdateEVec(vec);
            }
        }
    }
}