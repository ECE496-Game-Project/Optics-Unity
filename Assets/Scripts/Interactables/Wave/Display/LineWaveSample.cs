using System.Collections;
using UnityEngine;
using WaveUtils;
using CommonUtils;

namespace GO_Wave {
    public class LineWaveSample : MonoBehaviour {
        private GameObject _dispalyModule;

        //public bool DisplayStatus() {
        //    return _dispalyModule.activeSelf;
        //}

        //public void DisableDisplay() {
        //    _dispalyModule.SetActive(false);
        //}

        //public void EnableDisplay() {
        //    _dispalyModule.SetActive(true);
        //}

        public void UpdateEVec(Vector3 vec) {
            this.transform.LookAt((this.transform.position + this.transform.forward), vec);
            this.transform.localScale = new Vector3(1, vec.magnitude, 1);
        }

        private void Awake() {
            _dispalyModule = this.transform.Find("_dispalyModule").gameObject;
            if (_dispalyModule == null) {
                DebugLogger.Error(this.name, "Hierarchy collapse, Child GameObject Doesn't contains _dispalyModule, Stop Executing.");
            }
        }
    }
}