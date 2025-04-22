using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Vector3 currentVelocity; // Velocidade atual da câmera
    private bool canMove = true;
    public bool gameStarted = false;
    public float smoothSpeed = 0.125f; // Velocidade de suavização
    public float zOffset = -10f; // Deslocamento no eixo Z
    public float playerCatchUpSpeedMultiplier = 2f; // Multiplicador de velocidade para alcançar o jogador
    public Transform player; // Referência ao jogador

    private void Start()
    {
        PlayerEvents.onPlayerDied += StopCamera;
        PlayerEvents.onPlayerRevived += ResumeCamera;
    }

    private void OnDisable()
    {
        PlayerEvents.onPlayerDied -= StopCamera;
        PlayerEvents.onPlayerRevived -= ResumeCamera;
    }

    private void Update()
    {
        if (gameStarted == false) return; // idle the camera on game start   
        MoveForward(); // move the camera forward


        if (player != null)
        {
            // Verifica se o jogador está à frente da câmera
            if (player.position.z > transform.position.z - zOffset)
            {
                // Calcula a posição alvo da câmera para alcançar o jogador
                Vector3 targetPosition = transform.position;
                targetPosition.z = player.position.z + zOffset;

                // Acelera a câmera para alcançar o jogador
                transform.position = Vector3.Lerp(
                    transform.position,
                    targetPosition,
                    smoothSpeed * playerCatchUpSpeedMultiplier * Time.deltaTime
                );
            }
        }
    }

    public void StartCamera()
    {
        gameStarted = true;
    }

    public void MoveForward()
    {
        if (canMove) transform.position += currentVelocity * Time.deltaTime;
    }

    private void StopCamera() { canMove = false; }
    private void ResumeCamera() { canMove = true; }







}
