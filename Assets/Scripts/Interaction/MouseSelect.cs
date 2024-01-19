using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using Interfaces;
using Panel;
using UnityEngine.UIElements;
using System.Linq;

public class MouseSelect : MonoBehaviour
{
    [SerializeField] private PlayerInput m_playerInput;

    private GameObject m_highlight, m_select;

    private bool m_selectChangeOn = true;

    /* Check if MouseClick on UI */
    private List<VisualElement> m_expandPanels = new List<VisualElement>();

    public void TurnOn()
    {
        Assert.IsFalse(m_selectChangeOn);
        m_selectChangeOn = true;

    }

    public void TurnOff()
    {
        Assert.IsTrue(m_selectChangeOn);
        m_selectChangeOn = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_playerInput.actions["MouseClicked"].performed += OnMouseClicked;
        m_playerInput.actions["MouseMovement"].performed += onMouseMoved;

        //[TOOD]: UI Click Preparation
        foreach (var uidoc in FindObjectsOfType<UIDocument>()) {
            m_expandPanels.Add(uidoc.rootVisualElement.Q("ExpandPanel"));
        }
    }

    private void onMouseMoved(InputAction.CallbackContext context)
    {
        if (!m_selectChangeOn) return;

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
        if (!m_selectChangeOn) return;

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

        // [TODO]: Check if Mouse Click UI
        foreach (VisualElement expP in m_expandPanels) {
            VisualElement elementUnderMouse = expP.panel.Pick(Mouse.current.position.ReadValue());

            if (elementUnderMouse != null) {
                // Handle the click for the element
                Debug.Log("Clicked on element: " + elementUnderMouse.name);
                // Further processing...

                return;
            }
        }

        // if mouse is not on anything
        if (!Physics.Raycast(ray, out hit))
        {
            return;
        }

        GameObject go = hit.collider.gameObject;

        ISelectable clickable = go.GetComponent<ISelectable>();
        if (clickable != null)
        {
            OutlineManager.Instance.Highlight(go);
            m_highlight = go;
            m_select = go;

            clickable.OnMouseSelect();
        }

        // Announce ParamManager to show the corresponding panel
        I_ParameterTransfer paramUI = go.GetComponent<I_ParameterTransfer>();
        if (paramUI != null) {
            ParamPanelManager.Instance.SelectParamView(go);
        }
    }
}
