

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

public class UIController: InputController
{
    public override string m_name => "UIController";


    private static UIController m_instance=null;

    public static UIController Instance => m_instance;
    public UIController(InputController parent): base(parent)
    {
        Assert.IsNull(m_instance);
        m_instance = this;

    }

    public override void Update(float deltaTime)
    {
        if (!m_isAllowed) return;

        if (EventSystem.current.IsPointerOverGameObject())
        {
            NotifyMyParentIsOn();
        }
        else
        {
            NotifyMyParentIsFinished();
        }

        base.Update(deltaTime);
    }

}