using UnityEngine;
namespace GO_Wave
{
    public class WaveArrowController : MonoBehaviour
    {
        public Transform m_arrow;
        public Transform m_bar;
        public Transform m_arrowBot;

        private MeshRenderer m_arrowRenderer;
        private MeshRenderer m_barRenderer;

        private Vector3 m_arrow_offset;
        private Vector3 m_bar_offset;
        private Vector3 m_arrow_bottom_offset;

        private Vector3 m_bar_original_scale;

        private float m_rotZ=0f;
        public float RotZ => m_rotZ;

        // Start is called before the first frame update
        void Awake()
        {
            m_arrowRenderer = m_arrow.GetComponent<MeshRenderer>();
            m_barRenderer = m_bar.GetComponent<MeshRenderer>();

            m_arrow_offset = m_arrow.localPosition;
            m_bar_offset = m_bar.localPosition;

            m_arrow_bottom_offset = m_arrowBot.localPosition;
            m_bar_original_scale = m_bar.localScale;


        }

        public void UpdateMaterial(Material material)
        {
            m_arrow.GetComponent<MeshRenderer>().material = material;
            m_bar.GetComponent<MeshRenderer>().material = material;
        }

        public void UpdateTransform(float rotZ, float scale)
        {
            if (scale < 0) scale = 0;

            Quaternion rotArrow = Quaternion.Euler(new Vector3(m_arrow.localEulerAngles.x, m_arrow.localEulerAngles.y, rotZ));

            m_arrow.localPosition = Matrix4x4.Rotate(rotArrow).MultiplyVector(scale * m_arrow_bottom_offset + (m_arrow_offset - m_arrow_bottom_offset));
            m_arrow.localRotation = rotArrow;
            Quaternion rotBar = Quaternion.Euler(new Vector3(m_bar.localEulerAngles.x, m_bar.localEulerAngles.y, rotZ));
            Matrix4x4 scaleBar = Matrix4x4.Scale(new Vector3(m_bar.localScale.x, scale, m_bar.localScale.z));

            Matrix4x4 transformBar =  Matrix4x4.Rotate(rotBar) * scaleBar;

            m_bar.localPosition = transformBar.MultiplyVector(m_bar_offset);

            m_bar.localScale = new Vector3(m_bar_original_scale.x, scale, m_bar_original_scale.z);
            m_bar.localRotation = rotBar;
            //m_bar.localPosition = transformBar.GetPosition();
            if (scale == 0) m_arrow.gameObject.SetActive(false);
            else m_arrow.gameObject.SetActive(true);

            m_rotZ = rotZ;

        }

    }
}
