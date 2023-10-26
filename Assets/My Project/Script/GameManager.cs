using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform cameraTransform;
    public Transform playerTransform; // Assign the player's Transform in the Inspector.
    public KeyCode rotateLeftKey = KeyCode.Q;
    public KeyCode rotateRightKey = KeyCode.E;

    public float rotationAngle = 45f;
    public float rotationTime = 2f;
    public float cameraFollowSpeed = 5f; // Adjust this to control camera following speed.
    public float followDelay = 1f; // The delay in seconds before the camera starts following the player.

    private bool isRotating;
    private float followTimer;

    public bool IsRotating
    {
        get { return isRotating; }
    }

    void Start()
    {
        followTimer = followDelay;
    }

    void Update()
    {
        if (Input.GetKeyDown(rotateLeftKey) && !isRotating)
        {
            StartCoroutine(RotateCamera(rotationAngle));
        }

        if (Input.GetKeyDown(rotateRightKey) && !isRotating)
        {
            StartCoroutine(RotateCamera(-rotationAngle));
        }

        if (followTimer > 0)
        {
            followTimer -= Time.deltaTime;
        }
        else
        {
            // Ensure the camera follows the player's position smoothly
            if (playerTransform != null)
            {
                Vector3 targetPosition = playerTransform.position;
                targetPosition.z = cameraTransform.position.z; // Maintain the same z position
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, Time.deltaTime * cameraFollowSpeed);
            }
        }
    }

    IEnumerator RotateCamera(float targetRotation)
    {
        isRotating = true;
        foreach (PlayerController playerController in FindObjectsOfType<PlayerController>())
        {
            if (playerController.CanRotateCamera())
            {
                Quaternion startRotation = cameraTransform.rotation;
                Quaternion endRotation = Quaternion.Euler(0f, targetRotation, 0f) * startRotation;
                float elapsedTime = 0;

                while (elapsedTime < rotationTime)
                {
                    cameraTransform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / rotationTime);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                cameraTransform.rotation = endRotation;
                isRotating = false;
            }
        }
    }
}
