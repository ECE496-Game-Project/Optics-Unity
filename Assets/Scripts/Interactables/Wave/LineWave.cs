using UnityEngine;
using Profiles;
using CommonUtils;
using WaveDisplay;

namespace Wave {
    public class LineWave : MonoBehaviour {
        [Header("LineWave Parameter Settings")]
        [SerializeField] private WaveProfile AssignWaveSettings;
        [SerializeField] private LineWaveDisplay WaveDisplay;

        [SerializeField] private WaveParam ActiveWaveSettings;

        public void PreProcess(LineWave newLW, LineWaveDisplay wd, WaveParam wp) {

        }

        private void UpdateWaveLine() {
            for (int i = 0; i < WaveDisplay.m_SampleCount; i++) {
                WaveDisplay.UpdateSamplePoint(i, ActiveWaveSettings);
            }
        }
        /// <summary>
        /// <br>1. Update LineWave GO position with Wave Parameter's origin.</br>
        /// <br>2. Generate all sample Point Prefab.</br>
        /// </summary>
        private void SetupWaveLine() {
            this.transform.position = ActiveWaveSettings.Origin;
            //TODO: Add RayCast to Generate New Wave GO
            
            WaveDisplay.CreateSamplePoints(
                this.transform.position, 
                ActiveWaveSettings.KHat, 
                ActiveWaveSettings.VHat, 
                this.transform
            );
        }

        private bool RayCast(out float inLen, out float outLen) {
            inLen = WaveDisplay.PerSampleSpaceLength * WaveDisplay.m_SampleCount;
            outLen = 0;

            RaycastHit hit;
            if(Physics.Raycast(this.transform.position, ActiveWaveSettings.KHat, out hit, inLen)) {
                
                Object lwprefab = Resources.Load("Prefabs/LineWave/Pref_LineWave");

                Instantiate(lwprefab);


                outLen = inLen - hit.distance;
                inLen = hit.distance;

                return true;
            }
            return false;
        }

        // Use this for initialization
        void Start() {
            ActiveWaveSettings = new WaveParam(AssignWaveSettings.Parameters);

            SetupWaveLine();
        }

        // Update is called once per frame
        void Update() {
            UpdateWaveLine();
        }
    }
}