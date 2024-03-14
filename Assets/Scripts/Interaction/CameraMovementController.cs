
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovementController: InputController
{
    [SerializeField]
    private float m_movementSpeed = 20f;

    private Transform m_lookingObject;

    private PlayerInput m_playerInput;

    private Vector3 m_movingDir;

    public override string m_name => "CameraMovementController";

    private int processing = 0;
    public CameraMovementController(InputController parent, Transform lookingObject, PlayerInput playerInput): base(parent)
    {
        m_lookingObject = lookingObject;
        m_playerInput = playerInput;
        m_playerInput.actions["HorizontalPlaneMovement"].started += OnHorizontalPlaneMovementStart;
        m_playerInput.actions["HorizontalPlaneMovement"].performed += OnHorizontalPlaneMovement;
        m_playerInput.actions["HorizontalPlaneMovement"].canceled += OnHorizontalMovementFinished;

        m_playerInput.actions["VerticalMovement"].started += OnVerticalMovementStart;
        m_playerInput.actions["VerticalMovement"].performed += OnVerticalMovement;
        m_playerInput.actions["VerticalMovement"].canceled += OnVerticalMovementFinished;

        m_movingDir = Vector3.zero;


    }

    public void OnHorizontalPlaneMovementStart(InputAction.CallbackContext context)
    {
        if (!m_isAllowed) return;
        processing++;

        if (processing == 1)
        {
            NotifyMyParentIsOn();
        }
        
    }

    public void OnHorizontalPlaneMovement(InputAction.CallbackContext context)
    {
        if (!m_isAllowed || processing == 0) return;
        Vector2 delta = context.ReadValue<Vector2>();
        m_movingDir.x = delta.x;
        m_movingDir.z = delta.y;
    }

    public void OnHorizontalMovementFinished(InputAction.CallbackContext context)
    {
        if (!m_isAllowed || processing == 0) return;
        m_movingDir.x = 0;
        m_movingDir.z = 0;
        processing--;

        if (processing == 0 )
        {
            NotifyMyParentIsFinished();
        }
    }

    public void OnVerticalMovementStart(InputAction.CallbackContext context)
    {
        if (!m_isAllowed) return;
        processing++;
        if (processing == 1)
        {
            NotifyMyParentIsOn();
        }
    }

    public void OnVerticalMovement(InputAction.CallbackContext context)
    {
        if (!m_isAllowed || processing == 0) return;
        float delta = context.ReadValue<float>();

        m_movingDir.y = delta;
    }

    public void OnVerticalMovementFinished(InputAction.CallbackContext context)
    {
        if (!m_isAllowed || processing == 0) return;
        m_movingDir.y = 0;
        processing--;
        if (processing == 0)
        {
            NotifyMyParentIsFinished();
        }
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        if (m_movingDir == Vector3.zero) return;

        m_lookingObject.Translate(m_movingDir * m_movementSpeed * deltaTime, Space.Self);
    }
}