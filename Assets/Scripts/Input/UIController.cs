

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UIController: InputController
{
    public override string m_name => "UIController";
    private static UIController m_instance=null;

    public static UIController Instance => m_instance;
    public UIController(InputController parent): base(parent)
    {
        Assert.IsNull(m_instance);
        m_instance = this;


        GameObject cameraController = GameObject.Find("CameraController");
        if (cameraController == null)
        {
            Debug.LogError("CameraController not found");
            return;
        }
        PlayerInput playerInput = cameraController.transform.Find("PlayerInput").GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput not found");
            return;
        }

        TutorialController tutorialController = new TutorialController(this, playerInput);
        AddController(tutorialController);
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