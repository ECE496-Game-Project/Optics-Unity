using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class CameraRotateController: InputController
{

    public override string m_name => "CameraRotateController";

    private Transform m_lookingObject;

    private PlayerInput m_playerInput;

    [SerializeField]
    private float m_degreePerUnit = 10f;


    public CameraRotateController(InputController parent, Transform lookingObject, PlayerInput playerInput): base(parent)
    {
        m_lookingObject = lookingObject;
        m_playerInput = playerInput;
        m_playerInput.actions["Rotate"].started += OnRotateStarted;
        m_playerInput.actions["Rotate"].performed += OnRotatePerformed;
        m_playerInput.actions["Rotate"].canceled += OnRotateEnded;
    }

    public void OnRotateStarted(InputAction.CallbackContext context)
    {
        if (!m_isAllowed) return;
        NotifyMyParentIsOn();
    }

    private void OnRotatePerformed(InputAction.CallbackContext context)
    {
        if (!m_isAllowed) return;
        // record the mouse movement
        Vector2 delta = context.ReadValue<Vector2>();
        
        delta *= m_degreePerUnit * Time.deltaTime;

        // when mouse is moving to the right, it will rotate clockwise according to left hand coordinate systm
        m_lookingObject.Rotate(new Vector3(0, delta.x, 0f), Space.World);

        // when mouse is moving downward, it will rotate clockwise according to left hand coordinate systm
        m_lookingObject.Rotate(Vector3.right, -delta.y, Space.Self);

    }

    public void OnRotateEnded(InputAction.CallbackContext context)
    {
        if (!m_isAllowed) return;
        NotifyMyParentIsFinished();

    }
}
