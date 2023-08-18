using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemTest : MonoBehaviour
{

    private PlayerInput m_playerInput;

    // Start is called before the first frame update
    void Start()
    {
        m_playerInput = GetComponent<PlayerInput>();
    }

    public void Test()
    {

    }
    public void OnTranslation(InputAction.CallbackContext context)
    {
        Debug.Log($"OnMove: {context.ReadValue<Vector2>()}");
    }

    public void OnRotation(InputAction.CallbackContext context)
    {
        Debug.Log($"OnRotate: {context.ReadValue<Vector2>()}");
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        Debug.Log($"OnZoom: : {context.ReadValue<Vector2>()}");
    }
}
