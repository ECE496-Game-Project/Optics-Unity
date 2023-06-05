using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using CommonUtils;

namespace Wave {

    public class SingleWave : MonoBehaviour {
        public WaveProfile AssignWaveProfile;
        public GameObject SamplePointPrefab;
        
        [Header("WaveLine Settings")]
        public int SampleCount;
        public float Length;
        // Wave Origin is the position of this SingleWaveGO

        [SerializeField] private Transform[] SamplePointList;
        [SerializeField] private WaveParam ActiveParam;

        private void UpdateWaveLine() {
            Transform modifyTS;
            for (int i = 0; i < SampleCount; i++) {
                modifyTS = SamplePointList[i];
                Vector3 vec = waveAlgorithm.GetIrradiance(modifyTS.position, Time.time, ActiveParam);

                modifyTS.LookAt((modifyTS.position + modifyTS.forward), vec);
                modifyTS.localScale = new Vector3(1, vec.magnitude, 1);
            }
        }
        private void CreateWaveLine() {
            ActiveParam = AssignWaveProfile.waveParam;

            RaycastHit hit;

            Physics.Raycast(this.transform.position, ActiveParam.kHat, out hit, Length);
            //this.transform.position

            SamplePointList = new Transform[SampleCount];
            for (int i = 0; i < SampleCount; i++) {
                Vector3 initPos = this.transform.position + i * Length/SampleCount * ActiveParam.kHat;
                SamplePointList[i] = Instantiate(
                    SamplePointPrefab, 
                    initPos, 
                    Quaternion.LookRotation(ActiveParam.kHat, ActiveParam.vHat), 
                    this.transform
                ).transform;
            }
        }

        // Use this for initialization
        void Start() {
            CreateWaveLine();
        }

        // Update is called once per frame
        void Update() {
            UpdateWaveLine();
        }
    }
}