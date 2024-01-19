using UnityEngine;
using UnityEngine.Assertions;
using Interfaces;
using Panel;
using UnityEngine.UIElements;
using System.Linq;
using System.Collections.Generic;

public class SelectionController
{
    private GameObject m_highlight, m_select;

    private bool m_selectChangeOn = true;
    /* Check if MouseClick on UI */
    private List<VisualElement> m_expandPanels = new List<VisualElement>();
    private MouseInput m_mouseInput;

    public SelectionController(MouseInput mouseInput)
    {

        m_mouseInput = mouseInput;
        m_mouseInput.onMouseClicked.AddListener(OnMouseClicked);
        m_mouseInput.onMouseMoved.AddListener(OnMouseMoved);
    }

    ~SelectionController()
    {
        m_mouseInput?.onMouseClicked.RemoveListener(OnMouseClicked);
        m_mouseInput?.onMouseMoved.RemoveListener(OnMouseMoved);
    }

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

    private void OnMouseClicked(Vector2 mousePos)
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

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        RaycastHit hit;

        //// [TODO]: Check if Mouse Click UI
        //foreach (VisualElement expP in m_expandPanels) {
        //    VisualElement elementUnderMouse = expP.panel.Pick(Mouse.current.position.ReadValue());

        //    if (elementUnderMouse != null) {
        //        // Handle the click for the element
        //        Debug.Log("Clicked on element: " + elementUnderMouse.name);
        //        // Further processing...

        //        return;
        //    }
        //}

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

        OutlineManager.Instance.Highlight(go);
        m_highlight = go;
        m_select = go;

        clickable.OnMouseSelect();
    }

    private void OnMouseMoved(Vector2 mousePos)
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
}
