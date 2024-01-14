using System.Collections;
using UnityEngine;
using WaveUtils;
using CommonUtils;

namespace GO_Wave {
    public class LineWaveSample : MonoBehaviour {
        private GameObject _dispalyModule;

        private MeshRenderer _meshRenderer;

        private bool isFirst;
        public void UpdateEVec(Vector3 vec) {

            float oldAngle = transform.localEulerAngles.z;
            this.transform.LookAt((this.transform.position + this.transform.forward), vec);

            float newAngle = transform.localEulerAngles.z;

            this.transform.localScale = new Vector3(this.transform.localScale.x, vec.magnitude, this.transform.localScale.z);

            if (isFirst)
            {
                isFirst = false;
                return;
            }

            var newMaterial = TempSingletonManager.Instance.m_lineWaveSampleMaterialController.GetMaterial(oldAngle, newAngle);
            
            if (newMaterial != null)
            {
                if (_meshRenderer == null) _meshRenderer = _dispalyModule.GetComponent<MeshRenderer>();
                _meshRenderer.material = newMaterial;
            }
            
        }

        private void Awake() {
            _dispalyModule = this.transform.Find("_dispalyModule").gameObject;
            if (_dispalyModule == null) {
                DebugLogger.Error(this.name, "Hierarchy collapse, Child GameObject Doesn't contains _dispalyModule, Stop Executing.");
            }

            _meshRenderer = _dispalyModule.GetComponent<MeshRenderer>();
            if (_meshRenderer == null)
            {
                DebugLogger.Error(this.name, "Can not find MeshRenderer");
            }
        }
    }
}