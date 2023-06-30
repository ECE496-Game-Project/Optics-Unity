using System.Collections;
using UnityEngine;
using CommonUtils;
using WaveUtils;

namespace GO_Wave {
    public class WaveLineDisplay : MonoBehaviour {
        [Header("Line Wave Display Settings")]
        public int m_SampleCount;
        [SerializeField] private float _perSampleSpaceLength;
        [SerializeField] private GameObject _samplePointPrefab;
        private WaveLineSample[] _samplePointList;

        [Range(0.0f, 5.0f)]
        [SerializeField] private float _timeScale = 1.0f;

        private WaveSource _activeWS;

        private void Start() {
            _activeWS = GetComponent<WaveSource>();
            if (_activeWS == null) {
                DebugLogger.Error(this.name, "GameObject Doesn't contains WaveSource Script, Stop Executing.");
            }
            if (_samplePointPrefab.GetComponent<WaveLineSample>() == null) {
                DebugLogger.Error(this.name, "Prefab Doesn't contains WaveLineSample Script, Stop Executing.");
            }

            _samplePointList = new WaveLineSample[m_SampleCount];
            for (int i = 0; i < m_SampleCount; i++) {
                Vector3 initPos = this.transform.position + i * _perSampleSpaceLength * this.transform.forward;
                _samplePointList[i] = Object.Instantiate(
                    _samplePointPrefab,
                    initPos,
                    Quaternion.LookRotation(this.transform.forward, this.transform.up),
                    this.transform
                ).GetComponent<WaveLineSample>();
            }
        }

        private void Update() {
            for (int i = 0; i < m_SampleCount; i++) {
                Vector3 vec = WaveAlgorithm.CalcIrradiance(_samplePointList[i].transform.position - this.transform.position, Time.time * _timeScale, _activeWS.ActiveWaveParams);

                _samplePointList[i].UpdateEField(vec);
            }
        }
    }
}