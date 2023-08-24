using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseSelect : MonoBehaviour
{
    [SerializeField] private PlayerInput m_playerInput;

    [SerializeField] private Color m_outlineColor;
    [SerializeField] private float m_outlineWidth;

    private Outline m_highlight, m_select;



    // Start is called before the first frame update
    void Start()
    {
        m_playerInput.actions["MouseClicked"].performed += OnMouseClicked;
        m_playerInput.actions["MouseMovement"].performed += onMouseMoved;
    }

    private void onMouseMoved(InputAction.CallbackContext context)
    {
        // do nothing when currently selected object
        if (m_select != null) return;

        // disable the previous highlight
        if (m_highlight != null)
        {
            m_highlight.enabled = false;
            m_highlight = null;
        }

        Vector2 mousePos = m_playerInput.actions["MouseMovement"].ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        RaycastHit hit;

        // mouse is not on anything
        if (!Physics.Raycast(ray, out hit))
        {
            return;
            
        }

        GameObject go = hit.collider.gameObject;

        // it is not a selectable object
        IClickable clickable = go.GetComponent<IClickable>();
        if (clickable == null)
        {
            return;
        }

        Outline outline = go.GetComponent<Outline>();
        if (outline == null)
        {
            outline = go.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = m_outlineColor;
            outline.OutlineWidth = m_outlineWidth;
        }

        outline.enabled = true;
        m_highlight = outline;


    }

    private void OnMouseClicked(InputAction.CallbackContext context)
    {
        // turn off previous select
        if (m_select != null)
        {
            m_select.enabled = false;
            m_select = null;
        }

        // turn off previous select
        if (m_highlight != null)
        {
            m_highlight.enabled = false;
            m_highlight = null;
        }

        Vector2 mousePos = m_playerInput.actions["MouseMovement"].ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit))
        {
            if (m_select != null)
            {
                m_select.enabled = false;
                m_select = null;
            }
            return;
        }

        GameObject go = hit.collider.gameObject;

        IClickable clickable = go.GetComponent<IClickable>();
        if (clickable == null)
        {
            return;
        }

        Outline outline = go.GetComponent<Outline>();
        if (outline == null)
        {
            outline = go.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = m_outlineColor;
            outline.OutlineWidth = m_outlineWidth;
        }

        outline.enabled = true;
        m_highlight = outline;
        m_select = outline;

        clickable.OnMouseClicked();

    }
}
