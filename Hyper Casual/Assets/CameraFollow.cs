using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Referência ao jogador
    public float smoothSpeed = 0.125f; // Velocidade de suavização
    public float zOffset = -10f; // Deslocamento no eixo Z

    public Vector3 initialVelocity; // Velocidade inicial da câmera
    public Vector3 velocityIncrement; // Incremento de velocidade
    public float timeBetweenSpeedIncrement = 5f; // Intervalo entre aumentos de velocidade

    public float playerCatchUpSpeedMultiplier = 2f; // Multiplicador de velocidade para alcançar o jogador

    private Vector3 currentVelocity; // Velocidade atual da câmera

    private void Start()
    {
        currentVelocity = initialVelocity;
        StartCoroutine(IncrementalSpeed());
    }

    private void Update()
    {
        MoveForward();
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

    public void MoveForward()
    {
        // Move a câmera para frente com a velocidade atual
        transform.position += currentVelocity * Time.deltaTime;
    }

    private IEnumerator IncrementalSpeed()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenSpeedIncrement);
            currentVelocity += velocityIncrement;
        }
    }
}
