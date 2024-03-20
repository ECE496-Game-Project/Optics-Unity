using UnityEngine;
using CommonUtils;
using WaveUtils;
using Interfaces;
using SelectItems;
using System.Collections.Generic;

namespace GO_Wave {
    public partial class Wave : MonoBehaviour {
        public List<WaveSource> WaveSources;
        public static Wave NewLineWave(string name, List<WaveSource> sources, WaveParam roWP, LayerMask interactMask, float sampleRes, Vector3 position, Quaternion rotation) {
            GameObject new_GO = new GameObject(
                name,
                typeof(BoxCollider),
                typeof(Wave),
                typeof(LineWaveDisplay),
                typeof(LineWaveLogic),
                typeof(SelectableWave)
            );
            new_GO.GetComponent<Wave>().Init(sources, roWP);
            new_GO.GetComponent<LineWaveDisplay>().Init(sampleRes);
            new_GO.GetComponent<LineWaveLogic>().Init(interactMask);
            new_GO.transform.position = position;
            new_GO.transform.rotation = rotation;

            return new_GO.GetComponent<Wave>();
        }

        private I_WaveDisplay m_waveDisplay;
        public I_WaveDisplay WaveDisplay {
            get { if(m_waveDisplay==null) m_waveDisplay = GetComponent<I_WaveDisplay>(); return m_waveDisplay; }
        }

        private I_WaveLogic m_waveLogic;
        public I_WaveLogic WaveLogic {
            get { if (m_waveLogic == null) m_waveLogic = GetComponent<I_WaveLogic>(); return m_waveLogic; }
        }

        [SerializeField] private WaveParam m_params;
        public WaveParam Params {
            get { return m_params; }
            set {
                if (m_params != null)
                    DebugLogger.Error(this.name, "Re-Initalize WaveParameter! Break.");
                m_params = value;
            }
        }

        #region GLOBAL METHODS
        public void WaveVisualize() {
            WaveLogic.CleanInteract();
            WaveLogic.Interact();
            WaveDisplay.RefreshDisplay();
        }
        public void WaveClean() {
            WaveLogic.CleanInteract();
            WaveDisplay.CleanDisplay();
        }
        #endregion


        /// <summary>
        /// Script-Generated-WaveSource Requires to Call ManualAwake.
        /// </summary>
        /// <param name="srcWP"> Pre initalized WaveParameter.</param>
        public void Init(List<WaveSource> sources, WaveParam srcWP) {
            WaveSources = new List<WaveSource>(sources);
            m_params = srcWP;
        }

        public void Start() {
            WaveVisualize();
        }
    }
}