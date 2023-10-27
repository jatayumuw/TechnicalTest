using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    public float rotationSpeed = 2.0f;
    public float minYRotation = -30.0f;
    public float maxYRotation = 30.0f;
    public GameObject objectToRotate;

    void Update()
    {
        // Get the mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculate the rotation around the X-axis
        float rotationX = objectToRotate.transform.eulerAngles.x - mouseY * rotationSpeed;

        // Apply threshold limits on the Y-axis
        rotationX = Mathf.Clamp(rotationX, minYRotation, maxYRotation);

        // Rotate the object
        objectToRotate.transform.rotation = Quaternion.Euler(rotationX, objectToRotate.transform.eulerAngles.y + mouseX * rotationSpeed, 0);
    }
}