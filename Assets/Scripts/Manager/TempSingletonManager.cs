
using Cinemachine;
using CommonUtils;
using GO_Wave;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempSingletonManager : MonoSingleton<TempSingletonManager>
{
    [Header("User Input Parameters")]
    public SelectionController m_selectionController;
    public ZoomController m_zoomController;
    public DragMoveController m_dragMoveController;
    public CameraRotateController m_cameraRotateController;
    public CameraMovementController m_cameraMovementController;


    [Header("Referemce Variables")]
    public MouseInput m_mouseInput;

    [SerializeField] private PlayerInput m_playerInput;
    [SerializeField] private CinemachineVirtualCamera m_vcam;
    [SerializeField] private Transform m_lookingObject;

    [Header("Rendering")]
    public LineWaveSampleMaterialController m_lineWaveSampleMaterialController;

    //[Header("Wave Parameter")]
    //public ScaleManager m_scaleManager;

    protected override void Init()
    {
        base.Init();

        m_mouseInput = new MouseInput(m_playerInput);
        m_selectionController = new SelectionController(m_mouseInput);
        m_zoomController = new ZoomController(m_playerInput, m_vcam);
        m_dragMoveController = new DragMoveController(m_playerInput, m_zoomController, m_lookingObject);
        m_cameraRotateController = new CameraRotateController(m_lookingObject, m_playerInput);
        m_cameraMovementController = new CameraMovementController(m_lookingObject, m_playerInput);
    }

    private void Update()
    {
        WaveTime.Update(Time.deltaTime);
        m_cameraMovementController.Update(Time.deltaTime);
    }


}