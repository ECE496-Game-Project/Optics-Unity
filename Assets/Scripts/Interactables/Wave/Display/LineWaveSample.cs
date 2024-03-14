using System.Collections;
using UnityEngine;
using WaveUtils;
using CommonUtils;

namespace GO_Wave {
    public class LineWaveSample : MonoBehaviour {
        public WaveArrowController m_waveArrowController;
        [SerializeField] private GameObject _dispalyModule;

        private bool isFirst;
        public void UpdateEVec(Vector3 vec) {
            _dispalyModule.SetActive(true);

            float oldAngle = m_waveArrowController.RotZ;


            float scale = vec.magnitude;
            if(scale < WaveAlgorithm.FLOATROUNDING) {
                _dispalyModule.SetActive(false);
                return;
            }

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
    }
}