using System.Collections;
using UnityEngine;
using CommonUtils;

namespace WaveDisplay {
    [System.Serializable]
    public class LineWaveDisplay {
        [Header("LineWave Display Settings")]
        public int m_SampleCount;
        [SerializeField] private float _perSampleSpaceLength;
        [SerializeField] private GameObject _samplePointPrefab;
        private Transform[] _samplePointList;
        
        public float PerSampleSpaceLength {
            get { return _perSampleSpaceLength; }
        }

        public LineWaveDisplay(int SampleCount, float Length, GameObject SamplePointPrefab) {
            this.m_SampleCount = SampleCount;
            this._perSampleSpaceLength = Length;
            this._samplePointPrefab = SamplePointPrefab;
        }

        public LineWaveDisplay(LineWaveDisplay lwd) {
            this.m_SampleCount = lwd.m_SampleCount;
            this._perSampleSpaceLength = lwd._perSampleSpaceLength;
            this._samplePointPrefab = lwd._samplePointPrefab;
        }

        /// <summary>
        /// Create SamplePoints on the Line
        /// </summary>
        /// <param name="origin">Line Wave Origin</param>
        /// <param name="forward">Line Wave Forward Direction, same as kHat</param>
        /// <param name="up">Line Wave Up Direction, same as vHat</param>
        /// <param name="parent">Parent Object Stored under</param>
        public void CreateSamplePoints(Vector3 origin, Vector3 forward, Vector3 up, Transform parent) {
            _samplePointList = new Transform[m_SampleCount];
            for (int i = 0; i < m_SampleCount; i++) {
                Vector3 initPos = origin + i * _perSampleSpaceLength * forward;
                _samplePointList[i] = Object.Instantiate(
                    _samplePointPrefab,
                    initPos,
                    Quaternion.LookRotation(forward, up),
                    parent
                ).transform;
            }
        }

        /// <summary>
        /// Update SamplePointDisplay of a Single Sample Point.
        /// </summary>
        /// <param name="idx">Sample Point Index in SamplePointList</param>
        /// <param name="evec">Current SamplePoint's Electric Field Vector</param>
        public void UpdateSamplePoint(int idx, WaveParam wp) {
            Transform modifyTS;
            modifyTS = _samplePointList[idx];
            Vector3 vec = WaveAlgorithm.CalcIrradiance(modifyTS.position, Time.time, wp);

            modifyTS.LookAt((modifyTS.position + modifyTS.forward), vec);
            modifyTS.localScale = new Vector3(1, vec.magnitude, 1);
        }
    }
}