using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Rotation Attachments")]
    public Transform cameraTransform;
    public Transform playerTransform; // Assign the player's Transform in the Inspector.
    public KeyCode rotateLeftKey = KeyCode.Q;
    public KeyCode rotateRightKey = KeyCode.E;

    [Header("Rotation Parameters")]
    public float rotationAngle = 45f;
    public float rotationTime = 2f;
    public float cameraFollowSpeed = 5f; // Adjust this to control camera following speed.
    public float followDelay = 1f; // The delay in seconds before the camera starts following the player.

    private bool isRotating;
    private float followTimer;

    [Header("Player Respawn")]
    public KeyCode respawnKey = KeyCode.R; public CharacterController characterController;
    public Transform respawnPoint;  // Set this to the respawn point in the Inspector
    public int fallOfthreshold = -3;
    
    [Header("Player UI")]
    public KeyCode menuKey = KeyCode.Escape;
    public GameObject MenuPanel;
    public GameObject overlayPanel; // Assign the overlay panel in the Unity Inspector.
    public GameObject hudPanel; // Assign the HUD panel in the Unity Inspector.
    public bool gameStarted = false;


    public bool IsRotating
    {
        get { return isRotating; }
    }

    void Awake()
    {
        overlayPanel.SetActive(true);
        hudPanel.SetActive(false);
    }
    void Start()
    {
        followTimer = followDelay;
        MenuPanel.SetActive(false);
    }

    void Update()
    {
        if (!gameStarted && Input.anyKeyDown)
        {
            StartGame();
        }

        else if (gameStarted)
        {
            if (Input.GetKey(menuKey))
            {
                if (MenuPanel.activeSelf == true)
                    MenuPanel.SetActive(false);
                if (MenuPanel.activeSelf == false)
                    MenuPanel.SetActive(true);
            }

            if (Input.GetKeyDown(rotateLeftKey) && !isRotating)
                StartCoroutine(RotateCamera(rotationAngle));
            if (Input.GetKeyDown(rotateRightKey) && !isRotating)
                StartCoroutine(RotateCamera(-rotationAngle));

            if (playerTransform.position.y < fallOfthreshold || Input.GetKey(respawnKey))
            {
                characterController.enabled = false;
                playerTransform.position = respawnPoint.position;
                characterController.enabled = true;
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
    
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void CloseApplication()
    {
        Application.Quit();
    }

    void StartGame()
    {
        gameStarted = true;
        overlayPanel.SetActive(false);
        hudPanel.SetActive(true);
    }
}
