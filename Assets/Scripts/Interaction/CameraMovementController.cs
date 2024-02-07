
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovementController
{
    [SerializeField]
    private float m_movementSpeed = 20f;

    private Transform m_lookingObject;

    private PlayerInput m_playerInput;

    private Vector3 m_movingDir;
    public CameraMovementController(Transform lookingObject, PlayerInput playerInput)
    {
        m_lookingObject = lookingObject;
        m_playerInput = playerInput;
        m_playerInput.actions["HorizontalPlaneMovement"].performed += OnHorizontalPlaneMovement;
        m_playerInput.actions["HorizontalPlaneMovement"].canceled += OnHorizontalMovementFinished;
        m_playerInput.actions["VerticalMovement"].performed += OnVerticalMovement;
        m_playerInput.actions["VerticalMovement"].canceled += OnVerticalMovementFinished;

        m_movingDir = Vector3.zero;


    }

    public void OnHorizontalPlaneMovement(InputAction.CallbackContext context)
    {
        Vector2 delta = context.ReadValue<Vector2>();

        m_movingDir.x = delta.x;
        m_movingDir.z = delta.y;
    }

    public void OnHorizontalMovementFinished(InputAction.CallbackContext context)
    {
        m_movingDir.x = 0;
        m_movingDir.z = 0;
    }


    public void OnVerticalMovement(InputAction.CallbackContext context)
    {
        float delta = context.ReadValue<float>();

        m_movingDir.y = delta;
    }

    public void OnVerticalMovementFinished(InputAction.CallbackContext context)
    {
        m_movingDir.y = 0;
    }

    public void Update(float deltaTime)
    {

        if (m_movingDir == Vector3.zero) return;

        m_lookingObject.Translate(m_movingDir * m_movementSpeed * deltaTime, Space.Self);
    }
}