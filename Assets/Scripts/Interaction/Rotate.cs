using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rotate : MonoBehaviour
{

    [SerializeField]
    private Transform m_lookingObject;

    [SerializeField]
    private PlayerInput m_playerInput;

    [SerializeField]
    private float m_degreePerUnit = 10f;

    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
