using UnityEngine;
using UnityEngine.Assertions;
using Interfaces;
using Panel;
using UnityEngine.UIElements;
using System.Linq;
using System.Collections.Generic;
using SelectItems;
public class SelectionController : InputController
{
    private GameObject m_highlight, m_select;

    /* Check if MouseClick on UI */
    private List<VisualElement> m_expandPanels = new List<VisualElement>();
    private MouseInput m_mouseInput;

    public override string m_name => "SelectionController";

    public SelectionController(InputController parent, MouseInput mouseInput):base(parent)
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


    private void OnMouseClicked(Vector2 mousePos)
    {
        if (!m_isAllowed) return;

        // turn off previous select
        if (m_select != null)
        {
            //OutlineManager.Instance.UnHighlight(m_select);
            m_select.GetComponent<Selectable>()?.OnMouseUnselect();
            m_select = null;
        }

        // turn off previous select
        if (m_highlight != null)
        {
            //OutlineManager.Instance.UnHighlight(m_highlight);
            m_highlight.GetComponent<Selectable>()?.OnMouseUnhover();
            m_highlight = null;
        }

        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        RaycastHit hit;

        // if mouse is not on anything
        if (!Physics.Raycast(ray, out hit))
        {
            return;
        }

        GameObject go = hit.collider.gameObject;

        Selectable clickable = go.GetComponent<Selectable>();
        if (clickable != null)
        {
            //OutlineManager.Instance.Highlight(go);
            m_highlight = go;
            m_select = go;

            clickable.OnMouseSelect();
        }
    }

    private void OnMouseMoved(Vector2 mousePos)
    {
        if (!m_isAllowed) return;

        // do nothing when currently selected object
        if (m_select != null) return;

        // disable the previous highlight
        if (m_highlight != null)
        {
            //OutlineManager.Instance.UnHighlight(m_highlight.gameObject);
            m_highlight.GetComponent<Selectable>().OnMouseUnhover();
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
        Selectable clickable = go.GetComponent<Selectable>();
        if (clickable == null)
        {
            return;
        }


        //OutlineManager.Instance.Highlight(go);
        m_highlight = go;
        m_highlight.GetComponent<Selectable>().OnMouseHover();
    }
}
