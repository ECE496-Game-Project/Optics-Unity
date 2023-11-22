using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseSelect : MonoBehaviour
{
    [SerializeField] private PlayerInput m_playerInput;

    private GameObject m_highlight, m_select;



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
            OutlineManager.Instance.UnHighlight(m_highlight.gameObject);
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
        ISelectable clickable = go.GetComponent<ISelectable>();
        if (clickable == null)
        {
            return;
        }

        OutlineManager.Instance.Highlight(go);
        m_highlight = go;
        

    }

    private void OnMouseClicked(InputAction.CallbackContext context)
    {
        // turn off previous select
        if (m_select != null)
        {
            OutlineManager.Instance.UnHighlight(m_select);
            m_select.GetComponent<ISelectable>().OnMouseUnselect();
            m_select = null;
        }

        // turn off previous select
        if (m_highlight != null)
        {
            OutlineManager.Instance.UnHighlight(m_highlight);
            m_highlight = null;
        }

        Vector2 mousePos = m_playerInput.actions["MouseMovement"].ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        RaycastHit hit;

        // if mouse is not on anything
        if (!Physics.Raycast(ray, out hit))
        {
            return;
        }

        GameObject go = hit.collider.gameObject;

        ISelectable clickable = go.GetComponent<ISelectable>();
        if (clickable == null)
        {
            return;
        }

        OutlineManager.Instance.Highlight(go);
        m_highlight = go;
        m_select = go;

        clickable.OnMouseSelect();

    }
}
