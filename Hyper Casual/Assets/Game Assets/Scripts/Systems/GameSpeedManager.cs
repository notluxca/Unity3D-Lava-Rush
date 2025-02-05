using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Rendering.Universal;

public class GameSpeedManager : MonoBehaviour
{
    [SerializeField] private MovementHandler playerMovementHandler;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private float speedUpRate = 1.1f; // 10% de aumento
    [SerializeField] private float maxPlayerSpeed = 20f;
    [SerializeField] private float maxCameraSpeed = 15f;
    [SerializeField] private float timeBetweenSpeedIncrement = 5f;

    private float playerInitialSpeed;
    private float cameraInitialSpeed;
    private bool gameStarted = false;

    private void Awake()
    {
        PlayerEvents.OnPlayerFirstMove += PlayerFirstMove;
        playerInitialSpeed = playerMovementHandler.moveDuration;
        cameraInitialSpeed = cameraFollow.currentVelocity.z;
    }


    private void PlayerFirstMove(Vector3 position){
        if(!gameStarted) gameStarted = true;
        cameraFollow.StartCamera();
        PlayerEvents.OnPlayerFirstMove -= PlayerFirstMove;
        
        StartCoroutine(IncrementalSpeed()); // Start camera incremental speed
    }

    private IEnumerator IncrementalSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenSpeedIncrement);

            float newPlayerSpeed = playerMovementHandler.moveDuration * speedUpRate;
            float newCameraSpeed = cameraFollow.currentVelocity.z * speedUpRate;

            playerMovementHandler.moveDuration = Mathf.Min(newPlayerSpeed, maxPlayerSpeed);
            cameraFollow.currentVelocity.z = Mathf.Min(newCameraSpeed, maxCameraSpeed);
            Debug.Log($"Velocity changed {playerMovementHandler.moveDuration} {cameraFollow.currentVelocity.z}");
        }
    }
}
