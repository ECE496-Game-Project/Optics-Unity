using UnityEngine;
using CommonUtils;
using WaveUtils;
using Profiles;

namespace GO_Wave {
    public partial class WaveSource : MonoBehaviour {
        #region INSPECTOR SETTINGS
        [SerializeField] private SO_WaveParams _profile;
        [SerializeField] private LayerMask _interactMask;
        [SerializeField] private float _sampleResolution;
        #endregion

        private WaveSourceParam m_param;
        private Wave m_wave;
        
        public void Emit() {
            Close();
            m_wave = Wave.NewLineWave(
                this.name + "GenLineWave",
                new WaveParam(m_param), _interactMask, 
                _sampleResolution,
                this.transform.position + this.transform.forward * 0.01f, 
                this.transform.rotation
            );
        }
        public void Close() {
            if (m_wave != null) {
                m_wave.WaveClean();
                Destroy(m_wave.gameObject);
            }
        }


        private void Awake() {
            if(_profile == null)
                DebugLogger.Error(this.name, "RootWave Does not contain WaveProfile! Stop Executing.");

            m_param = new WaveSourceParam(_profile.Parameters);
            m_param.Origin = transform.position;
            m_param.UHat = transform.right;
            m_param.VHat = transform.up;
            m_param.KHat = transform.forward;
            
        }

        private void Start()
        {
            Emit();
        }
    }
}