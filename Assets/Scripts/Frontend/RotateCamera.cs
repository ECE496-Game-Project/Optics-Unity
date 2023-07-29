using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public void RotateToAxisX()
    {
        // Rotate the camera to the X-axis
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void RotateToAxisY()
    {
        // Rotate the camera to the Y-axis
        transform.rotation = Quaternion.Euler(0f, 90f, 0f);
    }

    public void RotateToAxisZ()
    {
        // Rotate the camera to the Z-axis
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) RotateToAxisX();
        if (Input.GetKeyDown(KeyCode.Y)) RotateToAxisY();
        if (Input.GetKeyDown(KeyCode.Z)) RotateToAxisZ();
    }
}

