using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class ZoomController: InputController
{
    public override string m_name => "ZoomController";
    private PlayerInput m_playerInput;


    private CinemachineVirtualCamera m_vcam;
    private CinemachineTransposer m_transposer;

    [SerializeField]
    private float m_zoomDelta = 1.5f;

    public float CameraDepth => -m_transposer.m_FollowOffset.z;

    public ZoomController(InputController manager, PlayerInput playerInput, CinemachineVirtualCamera vcam):base(manager)
    {
        m_playerInput = playerInput;
        m_vcam = vcam;

        m_playerInput.actions["Zoom"].performed += OnZoomPerformed;
        m_playerInput.actions["Zoom"].started += OnZoomStarted;
        m_playerInput.actions["Zoom"].canceled += OnZoomEnded;
        m_transposer = m_vcam.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineTransposer;
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    m_playerInput.actions["ZoomController"].performed += OnZoomPerformed;

    //    m_transposer = m_vcam.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineTransposer;
    //}

    private void OnZoomStarted(InputAction.CallbackContext context)
    {
        if (!m_isAllowed) return;
        NotifyMyParentIsOn();
        Debug.Log("Zoom Started " + Time.frameCount);
    }

    private void OnZoomPerformed(InputAction.CallbackContext context)
    {
        if (!m_isAllowed) return;
        float value = context.ReadValue<Vector2>().y;

        if (value < 0 )
        {
            m_transposer.m_FollowOffset.z -= m_zoomDelta;
        }
        else
        {
            m_transposer.m_FollowOffset.z += m_zoomDelta;
        }


        m_transposer.m_FollowOffset.z = Math.Clamp(m_transposer.m_FollowOffset.z, -float.MaxValue, -1f);

        Debug.Log("Zoom Processing " + Time.frameCount);

    }

    private void OnZoomEnded(InputAction.CallbackContext context)
    {
        if (!m_isAllowed) return;
        NotifyMyParentIsFinished();
        Debug.Log("Zoom Ended " + Time.frameCount);
    }

}
