using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    public void OnMouseSelect();
    public void OnMouseUnselect();
}