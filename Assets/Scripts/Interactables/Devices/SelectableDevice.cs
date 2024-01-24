using UnityEngine;

public class SelectableDevice : Selectable
{
    public override void OnMouseHover()
    {
        Debug.Log(gameObject.name + " is hovered");
    }

    public override void OnMouseSelect()
    {
        Debug.Log(gameObject.name + " is Select");
    }

    public override void OnMouseUnhover()
    {
        Debug.Log(gameObject.name + " is Unhover");
    }

    public override void OnMouseUnselect()
    {
        Debug.Log(gameObject.name + " is Unselect");
    }
}
