using UnityEngine;

public class GroundScroll : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Camera mainCamera; // Reference to the camera
    [SerializeField] private float moveSpeed = 1.0f; // Speed at which the object moves backward

    private Vector3 previousCameraPosition;

    void Start()
    {
        // If no camera is assigned, use the main camera
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Initialize the previous camera position
        if (mainCamera != null)
        {
            previousCameraPosition = mainCamera.transform.position;
        }
        else
        {
            Debug.LogError("No camera assigned or found!");
        }
    }

    void Update()
    {
        if (mainCamera == null) return;

        // Calculate the movement of the camera since the last frame
        Vector3 cameraMovement = mainCamera.transform.position - previousCameraPosition;

        // Move the object backward based on the camera's forward movement
        Vector3 backwardMovement = Vector3.back * moveSpeed;

        transform.position += backwardMovement * Time.deltaTime;

        // Update the previous camera position
        previousCameraPosition = mainCamera.transform.position;
    }
}
