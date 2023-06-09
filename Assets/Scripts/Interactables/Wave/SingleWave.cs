using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using CommonUtils;

namespace Wave {
    public class SingleWave : MonoBehaviour {
        [Header("LineWave Parameter Settings")]
        public WaveProfile AssignWaveSettings;
        [SerializeField] private WaveParam ActiveWaveSettings;

        public LineWaveDisplay WaveDisplay;

        private void UpdateWaveLine() {
            for (int i = 0; i < WaveDisplay.SampleCount; i++) {
                WaveDisplay.UpdateSamplePoint(i, ActiveWaveSettings);
            }
        }
        private void CreateWaveLine() {
            ActiveWaveSettings = AssignWaveSettings.waveParam;
            this.transform.position = ActiveWaveSettings.origin;

            //RaycastHit hit;
            //Physics.Raycast(this.transform.position, ActiveWaveSettings.kHat, out hit, SampleSettings.Length);
            
            WaveDisplay.CreateSamplePoints(
                this.transform.position, 
                ActiveWaveSettings.kHat, 
                ActiveWaveSettings.vHat, 
                this.transform
            );

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