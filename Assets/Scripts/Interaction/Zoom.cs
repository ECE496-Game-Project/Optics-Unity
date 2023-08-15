using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Zoom : MonoBehaviour
{
    [SerializeField]
    private PlayerInput m_playerInput;

    [SerializeField]
    private CinemachineVirtualCamera m_vcam;
    private CinemachineTransposer m_transposer;

    [SerializeField]
    private float m_zoomDelta = 1.5f;

    public float CameraDepth => -m_transposer.m_FollowOffset.z;

    // Start is called before the first frame update
    void Start()
    {
        m_playerInput.actions["Zoom"].performed += OnZoomPerformed;

        m_transposer = m_vcam.GetCinemachineComponent(CinemachineCore.Stage.Body) as CinemachineTransposer;
    }

    private void OnZoomPerformed(InputAction.CallbackContext context)
    {
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


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
