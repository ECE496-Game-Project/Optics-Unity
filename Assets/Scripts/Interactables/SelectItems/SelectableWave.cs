using UnityEngine;
using Panel;

public class SelectableWave : Selectable
{
    public override void OnMouseHover()
    {
        //Debug.Log(gameObject.name + " is hovered");
    }

    public override void OnMouseSelect()
    {
        ParamPanelManager.Instance.SelectParamView(this.gameObject);
    }

    public override void OnMouseUnhover()
    {
        //Debug.Log(gameObject.name + " is Unhover");
    }

    public override void OnMouseUnselect()
    {
        //Debug.Log(gameObject.name + " is Unselect");
    }
}