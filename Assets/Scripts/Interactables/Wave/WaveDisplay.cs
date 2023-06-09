using System.Collections;
using UnityEngine;

namespace Wave {
    [System.Serializable]
    public class LineWaveDisplay {
        [Header("LineWave Display Settings")]
        public int SampleCount;
        public float Length;
        public GameObject SamplePointPrefab;
        public Transform[] SamplePointList;

        /// <summary>
        /// Create SamplePoints on the Line
        /// </summary>
        /// <param name="origin">Line Wave Origin</param>
        /// <param name="forward">Line Wave Forward Direction, same as kHat</param>
        /// <param name="up">Line Wave Up Direction, same as vHat</param>
        /// <param name="parent">Parent Object Stored under</param>
        public void CreateSamplePoints(Vector3 origin, Vector3 forward, Vector3 up, Transform parent) {
            SamplePointList = new Transform[SampleCount];
            for (int i = 0; i < SampleCount; i++) {
                Vector3 initPos = origin + i * Length / SampleCount * forward;
                SamplePointList[i] = Object.Instantiate(
                    SamplePointPrefab,
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
            modifyTS = SamplePointList[idx];
            Vector3 vec = WaveAlgorithm.CalcIrradiance(modifyTS.position, Time.time, wp);

            modifyTS.LookAt((modifyTS.position + modifyTS.forward), vec);
            modifyTS.localScale = new Vector3(1, vec.magnitude, 1);
        }
    }
}