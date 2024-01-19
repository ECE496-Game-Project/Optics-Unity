using UnityEngine;
using UnityEngine.Assertions;
using Interfaces;

public class SelectionController
{
    private GameObject m_highlight, m_select;

    private bool m_selectChangeOn = true;

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
