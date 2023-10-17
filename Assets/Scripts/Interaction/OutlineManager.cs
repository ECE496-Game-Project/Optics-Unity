using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class OutlineManager : MonoBehaviour
{
    private static OutlineManager m_instance;
    public static OutlineManager Instance => m_instance;

    [SerializeField] private Color m_outlineColor;
    [SerializeField] private float m_outlineWidth;


    private void Start()
    {
        if (m_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        m_instance = this;
    }

    public void Highlight(GameObject go)
    {
        if (go == null) return;

        Outline outline = go.GetComponent<Outline>();

        if(outline == null)
        {
            outline = go.AddComponent<Outline>();
            outline.OutlineColor = m_outlineColor;
            outline.OutlineWidth = m_outlineWidth;
        }

        outline.enabled = true;
    }

    public void UnHighlight(GameObject go)
    {
        if (go == null) return;

        Outline outline = go.GetComponent<Outline>();
        Assert.IsNotNull(outline);

        if (outline == null) return;

        outline.enabled = false;
    }
}
