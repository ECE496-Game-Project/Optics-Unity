
using UnityEngine;

namespace GO_Wave
{

    [System.Serializable]
    public class LineWaveSampleMaterialController
    {


        [SerializeField]
        private Material m_positiveMaterial;
        [SerializeField]
        private Material m_negativeMaterial;

        //[SerializeField]
        //private float m_positiveRangeMin = -90f, m_positiveRangeMax = 90f;



        public Material PositiveMaterial => m_positiveMaterial;
        public Material NegativeMaterial => m_negativeMaterial;


        // if return null, it means keep the old material
        public Material GetMaterial(float oldAngle, float newAngle)
        {
            return m_positiveMaterial;
            //float deltaAngle = Mathf.Abs(Mathf.DeltaAngle(oldAngle, newAngle));


            //// if the deltaAngle is not 180, it means the polarization is spherical
            //if (!Mathf.Approximately(deltaAngle, 180f))
            //{
            //    return null;
            //}


            //float normalizedAngle = Mathf.DeltaAngle(newAngle, 0);

            //if (normalizedAngle >= m_positiveRangeMin && normalizedAngle < m_positiveRangeMax)
            //{
            //    return m_positiveMaterial;
            //}
            //else
            //{
            //    return m_negativeMaterial;
            //}
        }
        public Material SwapMaterial(Material material)
        {
            if (material == null)
            {
                Debug.LogError("Material is null");
                return null;
            }

            if (m_positiveMaterial == null || m_negativeMaterial == null)
            {
                Debug.LogError("Material is not initialize");
                return null;
            }

            if (material.color == m_positiveMaterial.color)
            {
                return m_negativeMaterial;
            }
            else if (material.color == m_negativeMaterial.color)
            {
                return m_positiveMaterial;
            }
            else
            {
                Debug.LogError("Material is not in the list");
                return null;
            }
        }   
    }

}