using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHighlightController : MonoBehaviour
{
    private GameObject m_highlightedObject;

    private void Start()
    {
        m_highlightedObject = null;
    }

    public void HighlightObject(GameObject obj)
    {
        m_highlightedObject = obj;
        OutlineManager.Instance.Highlight(obj);
    }

    public void UnhighlightObject()
    {
        if (m_highlightedObject != null)
        {
            OutlineManager.Instance.UnHighlight(m_highlightedObject);
            m_highlightedObject = null;
        }
    }
}
