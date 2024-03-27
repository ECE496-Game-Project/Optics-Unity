
using UnityEngine.InputSystem;
using UnityEngine;

public class GlobalController : InputController
{
    public override string m_name => "GlobalController";

    private MouseInput m_mouseInput;
    public GlobalController(MouseInput mouseInput): base(null)
    {
        m_mouseInput = mouseInput;

        CameraAbstractController cameraAbstractController = new CameraAbstractController(this);
        SelectionController selectionController = new SelectionController(this, m_mouseInput);
        
        UIController uiController = new UIController(this);
        AddController(selectionController);
        AddController(cameraAbstractController);
        AddController(uiController);
        
        AddControllerRelationship(selectionController.m_name, cameraAbstractController.m_name, false);
        
        AddControllerRelationship(selectionController.m_name, uiController.m_name, false);
        AddControllerRelationship(cameraAbstractController.m_name, uiController.m_name, false);
    }
}