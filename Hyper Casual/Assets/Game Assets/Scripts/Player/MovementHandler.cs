using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] private bool canMove = true;
    [SerializeField] public float moveDuration = 0.2f;
    [SerializeField] private float jumpHeight = 1.0f;
    
    private float horizontalGridSize;
    private float verticalGridSize;
    private Queue<System.Action> moveQueue = new Queue<System.Action>();
    private Vector3 currentPosition;
    private bool isMoving = false;
    IPlatform currentPlataform;
    
    //! Responsabilidades externas
    AnimationHandler animationHandler;
    private bool moved = false;

    public void Initialize()
    {
        horizontalGridSize = GameInfo.horizontalGridSize; 
        verticalGridSize = GameInfo.verticalGridSize;
        currentPosition = transform.position;
        animationHandler = GetComponent<AnimationHandler>();

        PlayerEvents.OnPlayerSwipeLeft += MoveLeft;
        PlayerEvents.OnPlayerSwipeRight += MoveRight;
        PlayerEvents.OnPlayerTap += MoveFront;
        PlayerEvents.onPlayerDiedByPlataformFall += TurnOffMove;
    }

    private bool IsValidJump(Vector3 position)
    {
        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, 20f))
        {
            if (!hit.collider.CompareTag("Plataform")) {}
            currentPlataform = hit.collider.GetComponent<IPlatform>();
            return true;
        }
        return false;
    }

    private void TurnOffMove()
    {
        canMove = false;
        PlayerEvents.PlayerDied();
    }

    public void Death()
    {
        PlayerEvents.PlayerDied();
    }

    public void Move(Vector3 newPosition)
    {
        if (IsValidJump(newPosition))
        {
            StartCoroutine(HandleMovement(newPosition, "Jump", false));
        }
        else
        {
            canMove = false;
            Invoke("Death", 0.4f);
            StartCoroutine(HandleMovement(newPosition, "Jump", true));
        }
    }

    private IEnumerator HandleMovement(Vector3 newPosition, string animationName, bool isDeath)
    {
        newPosition.y = isDeath ? -8.214834f : -7.4f;

        if (!moved)
        {
            moved = true;
            PlayerEvents.PlayerFirstMove(newPosition);
        }
        else
        {
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
            animationHandler.PlayRandomJump(animationName, 0, t);

            transform.position = position;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
        animationHandler.Play(isDeath ? "DeathAnimation" : "Idle");

        if (!isDeath)
        {
            currentPlataform.Jumped();
            PlayerEvents.PlayerColidedWithPlatform();
        }

        transform.position = newPosition;
        isMoving = false;

        if (moveQueue.Count > 0 && canMove) // Só processa a fila se ainda puder se mover
            moveQueue.Dequeue().Invoke();
        else
            moveQueue.Clear(); // Limpa a fila caso o movimento anterior tenha sido de morte

    }

    private void MoveLeft()
    {
        if (!canMove || isMoving)
        {
            if (isMoving) moveQueue.Enqueue(MoveLeft);
            return;
        }
        Vector3 newPosition = currentPosition + Vector3.left * horizontalGridSize + Vector3.forward * verticalGridSize;
        Move(newPosition);
    }

    private void MoveRight()
    {
        if (!canMove || isMoving)
        {
            if (isMoving) moveQueue.Enqueue(MoveRight);
            return;
        }
        Vector3 newPosition = currentPosition + Vector3.right * horizontalGridSize + Vector3.forward * verticalGridSize;
        Move(newPosition);
    }

    public void MoveFront()
    {
        if (!canMove || isMoving)
        {
            if (isMoving) moveQueue.Enqueue(MoveFront);
            return;
        }
        Vector3 newPosition = currentPosition + Vector3.forward * verticalGridSize;
        Move(newPosition);
    }

    public void VerifyQueueMove(Action function)
    {
        if (!canMove || isMoving)
        {
            if (isMoving) moveQueue.Enqueue(function);
            return;
        }
    }

    private void OnDisable()
    {
        PlayerEvents.OnPlayerSwipeLeft -= MoveLeft;
        PlayerEvents.OnPlayerSwipeRight -= MoveRight;
        PlayerEvents.OnPlayerTap -= MoveFront;
    }
}