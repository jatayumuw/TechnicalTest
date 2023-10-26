using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2.0f;
    public float runSpeed = 4.0f;
    public KeyCode runKey = KeyCode.LeftShift;

    private CharacterController characterController;
    private Transform mainCameraTransform;
    private GameManager gameManager;

    private bool isMoving = true;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        mainCameraTransform = Camera.main.transform;
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (!gameManager.IsRotating)
        {
            // Get input for movement
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            // Determine movement speed
            float moveSpeed = Input.GetKey(runKey) ? runSpeed : walkSpeed;

            // Calculate the movement vector based on camera's rotation
            Vector3 cameraForward = mainCameraTransform.forward;
            Vector3 cameraRight = mainCameraTransform.right;
            cameraForward.y = 0; // Ignore camera's vertical rotation
            cameraRight.y = 0;   // Ignore camera's vertical rotation

            Vector3 movement = (cameraForward.normalized * verticalInput + cameraRight.normalized * horizontalInput) * moveSpeed;

            // Apply movement to the character controller if not rotating
            if (!gameManager.IsRotating)
            {
                characterController.SimpleMove(movement);
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
        }
    }

    public bool CanRotateCamera()
    {
        return isMoving;
    }
}
