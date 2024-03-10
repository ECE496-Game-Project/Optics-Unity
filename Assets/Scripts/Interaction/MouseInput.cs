using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.Events;
using UnityEngine.Assertions;

public class MouseInput
{

    private static MouseInput m_instance=null;
    public static MouseInput Instance => m_instance;

    public UnityEvent<Vector2> onMouseMoved, onMouseClicked;

    private PlayerInput m_playerInput;

    public MouseInput(PlayerInput playerInput)
    {
        if (m_instance == null) m_instance  = this;
        else Assert.IsTrue(false, "MouseInput already exists");

        
        m_playerInput = playerInput;
        onMouseMoved = new UnityEvent<Vector2>();
        onMouseClicked = new UnityEvent<Vector2>();
        m_playerInput.actions["MouseClicked"].performed += OnMouseClicked;
        m_playerInput.actions["MouseMovement"].performed += OnMouseMoved;
    }


    private void OnMouseMoved(InputAction.CallbackContext context)
    {

        onMouseMoved?.Invoke(m_playerInput.actions["MouseMovement"].ReadValue<Vector2>());
        

    }

    private void OnMouseClicked(InputAction.CallbackContext context)
    {
        onMouseClicked?.Invoke(m_playerInput.actions["MouseMovement"].ReadValue<Vector2>());

    }
}
