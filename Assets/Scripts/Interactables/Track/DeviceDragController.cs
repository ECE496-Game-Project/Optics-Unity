//using Constraint;
//using static Constraint.WaveDeviceOrder;
//using GO_Device;
//using UnityEngine;
//using UnityEngine.Assertions;
//using UnityEngine.InputSystem;


//public class DeviceDragController : MonoBehaviour {
//    private WaveOrderManager m_waveOrderManager;

//    [SerializeField]
//    private Vector3 m_origin, m_maxRange;

//    private PlayerInput m_playerInput;
//    [SerializeField]
//    private DeviceOrderInfo m_target;

//    [SerializeField] private GameObject m_camera;

//    private bool m_isDragging = false;
//    public void Init(WaveOrderManager waveOrderManager, Vector3 origin, Vector3 maxRange) {
//        m_waveOrderManager = waveOrderManager;
//        m_origin = origin;
//        m_maxRange = maxRange;
//    }

//    public void SetTarget(DeviceOrderInfo target) {
//        m_target = target;
//    }

//    public void Clear() {
//        m_target = null;
//    }

//    private void Awake() {
//        m_playerInput = Object.FindObjectOfType<PlayerInput>();
//        Assert.IsNotNull(m_playerInput);
//        m_camera = GameObject.Find("Main Camera");
//        Assert.IsNotNull(m_camera);
//    }

//    private void OnEnable() {
//        m_playerInput.actions["DeviceOrderMove"].performed += OnDrag;
//        m_playerInput.actions["DeviceOrderMove"].canceled += Finish;
//    }

//    private void OnDisable() {
//        if (m_playerInput != null) {
//            m_playerInput.actions["DeviceOrderMove"].performed -= OnDrag;
//            m_playerInput.actions["DeviceOrderMove"].canceled -= Finish;
//        }

//    }

//    private void OnDrag(InputAction.CallbackContext context) {
//        if (m_target == null) return;

//        Vector3 mousePosition = context.ReadValue<Vector2>();
//        // just set the random depth value that is in front of the camera
//        mousePosition.z = 10;
//        Vector3 endpoint = Camera.main.ScreenToWorldPoint(mousePosition);

//        Vector3 cameraPosition = m_camera.transform.position;
//        Vector3 rayDir = (endpoint - cameraPosition).normalized;

//        RaycastHit hit;

//        bool keepGoing = false;

//        Vector3 rayOrigin = cameraPosition;
//        int originalLayer = m_target.device.gameObject.layer;
//        m_target.device.gameObject.layer = LayerMask.NameToLayer("Special Layer");

//        LayerMask mask = LayerMask.GetMask("Special Layer");

//        if (m_isDragging || Physics.Raycast(rayOrigin, rayDir, out hit, LayerMask.NameToLayer("Special Layer"))) {
//            keepGoing = true;
//        }

//        m_target.device.gameObject.layer = originalLayer;
//        if (!keepGoing) return;

//        var lineDir = (m_maxRange - m_origin).normalized;
//        var cameraDir = m_camera.transform.up;

//        Matrix4x4 matrix4X4 = new Matrix4x4();
//        matrix4X4.m00 = lineDir.x;
//        matrix4X4.m10 = lineDir.y;
//        matrix4X4.m20 = lineDir.z;
//        matrix4X4.m30 = 0;
//        matrix4X4.m01 = cameraDir.x;
//        matrix4X4.m11 = cameraDir.y;
//        matrix4X4.m21 = cameraDir.z;
//        matrix4X4.m31 = 0;
//        matrix4X4.m02 = -rayDir.x;
//        matrix4X4.m12 = -rayDir.y;
//        matrix4X4.m22 = -rayDir.z;
//        matrix4X4.m32 = 0;
//        matrix4X4.m03 = 0;
//        matrix4X4.m13 = 0;
//        matrix4X4.m23 = 0;
//        matrix4X4.m33 = 1;


//        if (matrix4X4.determinant == 0) {
//            Debug.Log("Determinant is 0");
//            return;
//        }

//        Matrix4x4 inverse = matrix4X4.inverse;
//        float dis = (matrix4X4.inverse.MultiplyVector(cameraPosition - m_origin)).x;

//        Vector3 lineIntersection = dis * lineDir + m_origin;

//        m_target.device.transform.position = Clamp(lineIntersection);

//        m_isDragging = true;
//    }

//    Vector3 Clamp(Vector3 target) {
//        float mag = (target - m_origin).magnitude;

//        if (mag > m_maxRange.magnitude) {
//            return m_maxRange;
//        }
//        else if (mag < 0) {
//            return m_origin;
//        }
//        else {
//            return target;
//        }
//    }
//    private void Finish(InputAction.CallbackContext context) {
//        if (m_target == null) return;

//        if (!m_isDragging) return;

//        int newIdx = m_waveOrderManager.GetNewIdx(m_target.device.transform.position, m_target.index);


//        m_waveOrderManager.ChangeIdx(m_target.index, newIdx);
//        m_isDragging = false;

//    }
//}
