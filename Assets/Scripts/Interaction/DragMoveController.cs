using UnityEngine;
using UnityEngine.InputSystem;

public class DragMoveController: InputController
{
    public override string m_name => "DragMoveController";

    private Transform m_lookingObject;

    private PlayerInput m_playerInput;

    private ZoomController m_zoomComponent;

    private Vector2 m_previousMousePosition;

    public DragMoveController(InputController parent, PlayerInput playerInput, ZoomController zoomController, Transform reference): base(parent)
    {
        m_playerInput = playerInput;
        m_zoomComponent = zoomController;
        m_lookingObject = reference;
        
        m_playerInput.actions["Translation"].started += OnMousePrep;
        m_playerInput.actions["Translation"].performed += OnMouseMove;
        m_playerInput.actions["Translation"].canceled += OnMouseFinished;
        
    }

    private void OnMousePrep(InputAction.CallbackContext context)
    {
        if (!m_isAllowed) return;
        m_previousMousePosition = context.ReadValue<Vector2>();

        NotifyMyParentIsOn();
    }


    public void OnMouseMove(InputAction.CallbackContext context)
    {
        if (!m_isAllowed) return;
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

    public void OnMouseFinished(InputAction.CallbackContext context)
    {
        
        if (!m_isAllowed) return;
        NotifyMyParentIsFinished();
    }
}
