using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class CameraRotateController
{

    private Transform m_lookingObject;

    private PlayerInput m_playerInput;

    [SerializeField]
    private float m_degreePerUnit = 10f;


    public CameraRotateController(Transform lookingObject, PlayerInput playerInput)
    {
        m_lookingObject = lookingObject;
        m_playerInput = playerInput;
        m_playerInput.actions["Rotate"].performed += OnRotatePerformed;
    }

    private void OnRotatePerformed(InputAction.CallbackContext context)
    {

        // record the mouse movement
        Vector2 delta = context.ReadValue<Vector2>();
        
        delta *= m_degreePerUnit * Time.deltaTime;

        // when mouse is moving to the right, it will rotate clockwise according to left hand coordinate systm
        m_lookingObject.Rotate(new Vector3(0, delta.x, 0f), Space.World);

        // when mouse is moving downward, it will rotate clockwise according to left hand coordinate systm
        m_lookingObject.Rotate(Vector3.right, -delta.y, Space.Self);
    }

}
