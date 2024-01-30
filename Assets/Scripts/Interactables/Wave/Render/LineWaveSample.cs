using System.Collections;
using UnityEngine;
using WaveUtils;
using CommonUtils;

namespace GO_Wave {
    public class LineWaveSample : MonoBehaviour {


        public WaveArrowController m_waveArrowController;

        private bool isFirst;
        public void UpdateEVec(Vector3 vec) {

            float oldAngle = m_waveArrowController.RotZ;


            float scale = vec.magnitude;
            float newAngle = Mathf.Acos(vec.y / scale) * Mathf.Rad2Deg;
            if (Vector3.Cross(vec, Vector3.up).z < 0)
            {
                newAngle = -newAngle;
            }

            m_waveArrowController.UpdateTransform(newAngle, scale);

            if (isFirst)
            {
                isFirst = false;
                return;
            }

            
            var newMaterial = TempSingletonManager.Instance.m_lineWaveSampleMaterialController.GetMaterial(oldAngle, newAngle);
            
            if (newMaterial != null)
            {
                m_waveArrowController.UpdateMaterial(newMaterial);
            }
            
        }

        private void Awake() {
            //_dispalyModule = this.transform.Find("_dispalyModule").gameObject;
            //if (_dispalyModule == null) {
            //    DebugLogger.Error(this.name, "Hierarchy collapse, Child GameObject Doesn't contains _dispalyModule, Stop Executing.");
            //}

            //_meshRenderer = _dispalyModule.GetComponent<MeshRenderer>();
            //if (_meshRenderer == null)
            //{
            //    DebugLogger.Error(this.name, "Can not find MeshRenderer");
            //}
        }
    }
}