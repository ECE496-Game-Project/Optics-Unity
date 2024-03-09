
using Cinemachine;
using CommonUtils;
using GO_Wave;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempSingletonManager : MonoSingleton<TempSingletonManager>
{
    [Header("User Input Parameters")]
    public MouseInput m_mouseInput;
    public GlobalController m_globalController;

    [Header("Referemce Variables")]
    
    [SerializeField] private PlayerInput m_playerInput;


    [Header("Rendering")]
    public LineWaveSampleMaterialController m_lineWaveSampleMaterialController;

    [Header("Wave Parameter")]
    public ScaleManager m_scaleManager;
    protected override void Init()
    {
        base.Init();
        GameObject cameraController = GameObject.Find("CameraController");
        if (cameraController == null)
        {
            Debug.LogError("CameraController not found");
            return;
        }

        PlayerInput playerInput = cameraController.transform.Find("PlayerInput").GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput not found");
            return;
        }

        m_playerInput = playerInput;
        m_mouseInput = new MouseInput(m_playerInput);

        m_globalController = new GlobalController(m_mouseInput);
        
    }

    private void Update()
    {
        m_globalController.Update(Time.deltaTime);
    }


}