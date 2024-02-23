


using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class CameraAbstractController : InputController
{
    public override string m_name => "CameraAbstractController";

    public CameraAbstractController(InputController manager) : base(manager)
    {
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

        Transform lookingObject = cameraController.transform.Find("Looking Object");
        if (lookingObject == null)
        {
            Debug.LogError("Looking Object not found");
            return;
        }

        CinemachineVirtualCamera virtualCamera = cameraController.transform.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera == null)
        {
            Debug.LogError("CinemachineVirtualCamera not found");
            return;
        }

        ZoomController zoomController = new ZoomController(this, playerInput, virtualCamera);
        CameraRotateController rotateController = new CameraRotateController(this, lookingObject, playerInput);
        DragMoveController dragMoveController = new DragMoveController(this, playerInput, zoomController, lookingObject);

        AddController(zoomController);
        AddController(rotateController);
        AddController(dragMoveController);

        AddControllerRelationship(zoomController.m_name, rotateController.m_name, false);
        AddControllerRelationship(zoomController.m_name, dragMoveController.m_name, false);
        AddControllerRelationship(rotateController.m_name, dragMoveController.m_name, false);
    }

    
}