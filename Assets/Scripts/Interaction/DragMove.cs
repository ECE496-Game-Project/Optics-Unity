using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragMove : MonoBehaviour
{

    [SerializeField]
    private Transform m_lookingObject;

    [SerializeField]
    private PlayerInput m_playerInput;

    [SerializeField]
    private Zoom m_zoomComponent;

    private Vector2 m_previousMousePosition;
    // Start is called before the first frame update
    void Start()
    {
        m_zoomComponent = transform.GetComponent<Zoom>();
        m_playerInput.actions["Translation"].performed += OnMouseMove;
        m_playerInput.actions["Translation"].started += OnMousePrep;
    }

    private void OnMousePrep(InputAction.CallbackContext context)
    {

        m_previousMousePosition = context.ReadValue<Vector2>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        Vector3 mousePosition = context.ReadValue<Vector2>();

        // set the depth to where the looking object is
        mousePosition.z = m_zoomComponent.CameraDepth;

        Vector3 endpoint = Camera.main.ScreenToWorldPoint(mousePosition);

        // find the previous location corresponding to the previous mouse position
        Vector3 prevMousePosition = m_previousMousePosition;
        prevMousePosition.z = m_zoomComponent.CameraDepth;
        Vector3 prevEndpoint = Camera.main.ScreenToWorldPoint(prevMousePosition);

        // find the translation that the object need to move
        Vector3 translation = endpoint - prevEndpoint;

        m_lookingObject.Translate(-translation, Space.World);
        m_previousMousePosition = mousePosition;
    }
}
