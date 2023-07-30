using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public void RotateToAxisX()
    {
        // Rotate the camera to the X-axis
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.position = new Vector3(0, 1, -10);
    }

    public void RotateToAxisY()
    {
        // Rotate the camera to the Y-axis
        transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        transform.position = new Vector3(-10, 1, 6);
    }

    public void RotateToAxisZ()
    {
        // Rotate the camera to the Z-axis
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        transform.position = new Vector3(0, 10, 6);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) RotateToAxisX();
        if (Input.GetKeyDown(KeyCode.Y)) RotateToAxisY();
        if (Input.GetKeyDown(KeyCode.Z)) RotateToAxisZ();
    }
}

