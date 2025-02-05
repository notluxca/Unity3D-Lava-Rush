using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementHandler : MonoBehaviour
{
    // Receber a solicicação de movimento (Com Direção)
    // Verificar se o movimento é válido
    // Realizar o movimento
    // ela segue o open closed priciple? ou seja, ela é aberta a extensões e fechada a modificações? se eu quiser implementar uma nova maneira de se movimentar
    // eu teria que alterar este código? sim! então ela não segue o principio

    //! Esta fazendo gerenciamento de Actions e Eventos
    //! Está fazendo o gerenciamento da animação (por mais que isso possa ser necessário, não é responsabilidade do movimento)

    // Essential variables
    [SerializeField] private bool canMove = true;
    [SerializeField] public float moveDuration = 0.2f;
    [SerializeField] private float jumpHeight = 1.0f;
    
    private float horizontalGridSize;
    private float verticalGridSize;
    private Queue<System.Action> moveQueue = new Queue<System.Action>();
    private Vector3 currentPosition;
    private bool isMoving = false;
    
    
    
    //! Responsabilidades externas
    AnimationHandler animationHandler; // ! Deveria ser removido
    private bool moved = false;
    
    
    


    public void Initialize()
    {
        horizontalGridSize = GameInfo.horizontalGridSize; // Depender de um singleton não é uma boa prática
        verticalGridSize = GameInfo.verticalGridSize;
        

        currentPosition = transform.position;
        animationHandler = GetComponent<AnimationHandler>(); //! Responsabilidade externa

        PlayerEvents.OnPlayerSwipeLeft += MoveLeft;
        PlayerEvents.OnPlayerSwipeRight += MoveRight;
        PlayerEvents.OnPlayerTap += MoveFront;
    }

    private void CheckJumpPosition(Vector3 position)
    {
        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, 20f))
        {
            Debug.DrawRay(position, Vector3.down * hit.distance, Color.green, 10);
            if (!hit.collider.CompareTag("Plataform"))
                Debug.Log("Movimento ilegal");
                // canMove = false;
                // StartCoroutine(FastLost());
                
        }
        else
        {
            PlayerEvents.PlayerDied();
            canMove = false;
        }
    }


    private IEnumerator MoveToPosition(Vector3 newPosition)
    {
        if (!moved)
        {
            moved = true;
            PlayerEvents.PlayerFirstMove(newPosition);
            Debug.Log("Player First Move");
        } else {
            PlayerEvents.PlayerMoved(newPosition);    
        }
        

        isMoving = true;
        Vector3 startPosition = transform.position;
        currentPosition = newPosition;
        float elapsedTime = 0;
        Quaternion startRotation = Quaternion.Euler(0, 0, 0);
        transform.rotation = startRotation;

        Vector3 direction = (newPosition - startPosition).normalized;
        float tiltAngle = direction.x > 0.5f ? 15f : direction.x < -0.5f ? -15f : 0f;

        Quaternion targetRotation = Quaternion.Euler(0, tiltAngle * 3, tiltAngle * 2);

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;

            Vector3 position = Vector3.Lerp(startPosition, newPosition, t);
            position.y = Mathf.Sin(t * Mathf.PI) * jumpHeight + Mathf.Min(startPosition.y, position.y);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            animationHandler.PlayRandomJump("Jump", 0, t);

            transform.position = position;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
        animationHandler.Play("Idle");
        transform.position = newPosition;
        isMoving = false;

        if (moveQueue.Count > 0)
            moveQueue.Dequeue().Invoke();
    }

    // Responsavel por dar comando de movimentação ao character enquanto o input é resolvido externamente
    private void MoveLeft(){
        if (!canMove || isMoving) 
        {
            if (isMoving) moveQueue.Enqueue(MoveLeft);
            return;
        }
        Vector3 newPosition = currentPosition + Vector3.left * horizontalGridSize + Vector3.forward * verticalGridSize;
        CheckJumpPosition(newPosition);
        StartCoroutine(MoveToPosition(newPosition));
    }

    private void MoveRight(){
        if (!canMove || isMoving) 
        {
            if (isMoving) moveQueue.Enqueue(MoveRight);
            return;
        }
        Vector3 newPosition = currentPosition + Vector3.right * horizontalGridSize + Vector3.forward * verticalGridSize;
        CheckJumpPosition(newPosition);
        StartCoroutine(MoveToPosition(newPosition));
    }

        public void MoveFront()
    {
        if (!canMove || isMoving) 
        {
            if (isMoving) moveQueue.Enqueue(MoveFront);
            return;
        }
        Vector3 newPosition = currentPosition + Vector3.forward * verticalGridSize;
        CheckJumpPosition(newPosition);
        StartCoroutine(MoveToPosition(newPosition));
    }

    public void VerifyQueueMove(Action function){
        if (!canMove || isMoving) 
        {
            if (isMoving) moveQueue.Enqueue(item: function);
            return;
        }
    }
    private void OnDisable() {
        PlayerEvents.OnPlayerSwipeLeft -= MoveLeft;
        PlayerEvents.OnPlayerSwipeRight -= MoveRight;
        PlayerEvents.OnPlayerTap -= MoveFront;
    }


}

