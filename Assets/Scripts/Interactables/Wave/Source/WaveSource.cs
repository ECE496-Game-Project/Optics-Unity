using UnityEngine;
using CommonUtils;
using WaveUtils;
using Profiles;
using Interfaces;
using ParameterTransfer;

namespace GO_Wave {
    public class WaveSource : MonoBehaviour {
        //public override string ParamTransferName { get { return "RootWave"; } }

        #region INSPECTOR SETTINGS
        [SerializeField] private SO_WaveParams _profile;
        [SerializeField] private LayerMask _interactMask;
        [SerializeField] private float _sampleResolution;
        #endregion

        private void Awake() {
            if(_profile == null)
                DebugLogger.Error(this.name, "RootWave Does not contain WaveProfile! Stop Executing.");
            _profile.Parameters.Origin = transform.position;
            _profile.Parameters.UHat = transform.right;
            _profile.Parameters.VHat = transform.up;
            _profile.Parameters.KHat = transform.forward;
            Wave.NewLineWave(new WaveParam(_profile.Parameters), _interactMask, _sampleResolution);
        }
    }
}