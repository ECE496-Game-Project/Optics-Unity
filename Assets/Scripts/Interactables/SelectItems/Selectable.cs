using UnityEngine;

public abstract class Selectable : MonoBehaviour
{
    public abstract void OnMouseSelect();
    public abstract void OnMouseUnselect();
    public abstract void OnMouseHover();
    public abstract void OnMouseUnhover();
}