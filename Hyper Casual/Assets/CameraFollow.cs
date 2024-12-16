using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // Referência ao jogador
    public float smoothSpeed = 0.125f; // Velocidade de suavização
    public float zOffset = -10f; // Deslocamento no eixo Z


    public Vector3 initialVelocity;
    // public float currentVelocity;
    public Vector3 velocityIncrement; // Velocidade de suavização
    public float timeBetweenSpeedIncrement = 5; // Velocidade de suavização

    public Vector3 currentVelocity;

    private void Start() {
        currentVelocity = initialVelocity;
        StartCoroutine(IncrementalSpeed());
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // // Calcula a nova posição da câmera
            // Vector3 targetPosition = transform.position;
            // targetPosition.z = player.position.z + zOffset;
            // // Suaviza a movimentação no eixo Z
            // transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothSpeed);
        }
    }

    private void FixedUpdate() {
        MoveFront();
    }

    public void MoveFront(){
        transform.position += currentVelocity;
    }

    IEnumerator IncrementalSpeed(){
        while(true){
            yield return new WaitForSeconds(5);
            currentVelocity += velocityIncrement;
        };
    }
    
    
}
