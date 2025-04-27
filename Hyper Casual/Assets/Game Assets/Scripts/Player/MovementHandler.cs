using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] private bool canMove = true;
    [SerializeField] public float moveDuration = 0.2f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] LayerMask plataformLayerMask;

    public float horizontalGridSize { get; private set; }
    public float verticalGridSize { get; private set; }
    private Queue<System.Action> moveQueue = new Queue<System.Action>();
    private Vector3 currentPosition;
    private bool isMoving = false;
    IPlatform currentPlataform;

    private bool alreadyDied = false;

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

        Debug.DrawRay(position, Vector3.down * 20f, Color.red, 10f);
        position.y = 5;
        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, 20f, plataformLayerMask))
        {

            currentPlataform = hit.collider.GetComponent<IPlatform>();
            return true;
        }
        return false;
    }

    private void TurnOffMove()
    {
        canMove = false;
        // PlayerEvents.PlayerDied();
    }

    private void TurnOnMove()
    {
        canMove = true;
    }

    public void Death()
    {
        CallDeathOnce();
    }

    // --- centraliza chamada segura de morte ---
    private void CallDeathOnce()
    {
        if (alreadyDied) return;
        alreadyDied = true;
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
            Debug.Log("Invalid jump to position: " + newPosition);
            canMove = false;
            Invoke("Death", 0.4f); // delay event call to give time to player fall on lava
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

    public void RevivePlayer()
    {
        CancelInvoke("Death"); // cancela a morte agendada

        Vector3 playerDeathPosition = transform.position;
        playerDeathPosition.z += GameInfo.verticalGridSize; // move up on grid position
        Vector3 firstRayPos = new Vector3(-GameInfo.verticalGridSize, 0, playerDeathPosition.z);
        // partindo 

        // find a safe landing spot
        for (int i = 0; i <= 2; i++)
        {

            if (Physics.Raycast(firstRayPos, Vector3.down, out RaycastHit hit, 20f))
            {
                if (hit.collider.CompareTag("Plataform"))
                {
                    hit.collider.gameObject.GetComponent<Platform>().SetFallTime(10);
                    Vector3 landingPosition = hit.point + Vector3.up * 0.5f; // levar em consideração altura do character
                    //todo: investigar hitpoint descentralizado
                    Move(hit.point + new Vector3(1.5f, 0, 0)); //! Esse offset serve para alinhar o player com a plataforma, por algum motivo o hit point é desalinhado
                    PlayerEvents.PlayerRevived();
                    alreadyDied = false; // <-- RESET morte depois do revive
                    canMove = true;
                    return;
                }
            }
            firstRayPos.x += GameInfo.horizontalGridSize;
        }

    }
}