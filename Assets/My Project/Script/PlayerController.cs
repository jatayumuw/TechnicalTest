using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2.0f;
    public float runSpeed = 4.0f;
    public KeyCode runKey = KeyCode.LeftShift;

    public Transform respawnPoint;  // Set this to the respawn point in the Inspector

    private CharacterController characterController;
    private Transform mainCameraTransform;
    private GameManager gameManager;
    private Animator playerAnimator;

    public float idleCounter = 0f;
    public float idleLimit = 5f;
    private bool isSprinting = false;

    private void Start()
    {
        characterController = GetComponent <CharacterController>();
        mainCameraTransform = Camera.main.transform;
        gameManager = FindObjectOfType <GameManager>();
        playerAnimator = GetComponent<Animator>();

        playerAnimator.SetBool("isIdling", true);
    }

    private void Update()
    {
        if (!gameManager.IsRotating)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            float moveSpeed = Input.GetKey(runKey) ? runSpeed : walkSpeed;

            Vector3 cameraForward = mainCameraTransform.forward;
            Vector3 cameraRight = mainCameraTransform.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            Vector3 movement = (cameraForward.normalized * verticalInput + cameraRight.normalized * horizontalInput) * moveSpeed;

            if (!gameManager.IsRotating)
            {
                bool isMoving = movement.magnitude > 0;

                if (isMoving)
                {
                    // Calculate the target rotation
                    Vector3 targetDirection = movement.normalized;
                    targetDirection.y = 0; // Lock rotation to the XZ plane
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

                    // Rotate the character towards the target direction
                    transform.rotation = targetRotation;

                    // Move the character forward
                    characterController.SimpleMove(transform.forward * moveSpeed);
                }

                idleCounter = isMoving ? 0f : idleCounter + Time.deltaTime;

                playerAnimator.SetBool("isWalking", isMoving && !isSprinting);
                playerAnimator.SetBool("isSprinting", isSprinting);
                playerAnimator.SetBool("isIdling", idleCounter > idleLimit);

                // Detect sprinting
                isSprinting = Input.GetKey(runKey);

                // Check if the player has fallen
                if (transform.position.y <= -5) // Change the Y threshold as needed
                {
                    Respawn();
                }
            }
        }
    }

    private void Respawn()
    {
        // Reset the player's position to the respawn point
        transform.position = respawnPoint.position;
        // Reset any other necessary variables or components
    }

    public bool CanRotateCamera()
    {
        return !isSprinting;
    }
}
