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
        private Transform[] _samplePointList;

        [Range(0.0f, 5.0f)]
        [SerializeField] private float _timeScale = 1.0f;

        public float TimeScaleGetter() {
            return _timeScale;
        }

        void Start() {
            if(_samplePointPrefab.GetComponent<WaveLineSample>() == null) {
                DebugLogger.Error(this.name, "Prefab Doesn't contains WaveLineSample Script, Stop Executing.");
            }

            _samplePointList = new Transform[m_SampleCount];
            for (int i = 0; i < m_SampleCount; i++) {
                Vector3 initPos = this.transform.position + i * _perSampleSpaceLength * this.transform.forward;
                _samplePointList[i] = Object.Instantiate(
                    _samplePointPrefab,
                    initPos,
                    Quaternion.LookRotation(this.transform.forward, this.transform.up),
                    this.transform
                ).transform;

                /*Init LineWaveSample*/
                _samplePointList[i].GetComponent<WaveLineSample>().init(
                    this.name + "_Sample[" + i + "]",
                    this.GetComponent<WaveSource>().ActiveWaveParams,
                    TimeScaleGetter
                );
            }
        }
    }
}