using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    // Essential variables
    [SerializeField] private bool canMove = true;
    [SerializeField] public float moveDuration = 0.2f;
    [SerializeField] private float jumpHeight = 1.0f;
    
    private float horizontalGridSize;
    private float verticalGridSize;
    private Queue<System.Action> moveQueue = new Queue<System.Action>();
    private Vector3 currentPosition;
    private bool isMoving = false;
    Plataform currentPlataform;
       
    //! Responsabilidades externas
    AnimationHandler animationHandler; // ! Deveria ser removido
    private bool moved = false;

    public void Initialize()
    {
        horizontalGridSize = GameInfo.horizontalGridSize; 
        verticalGridSize = GameInfo.verticalGridSize;
        currentPosition = transform.position;
        animationHandler = GetComponent<AnimationHandler>(); //! Responsabilidade externa

        PlayerEvents.OnPlayerSwipeLeft += MoveLeft;
        PlayerEvents.OnPlayerSwipeRight += MoveRight;
        PlayerEvents.OnPlayerTap += MoveFront;
    }

    private bool IsValidJump(Vector3 position)
    {
        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, 20f))
        {
            if (!hit.collider.CompareTag("Plataform")) {}
            currentPlataform = hit.collider.GetComponent<Plataform>();
            return true;
        }
        else
        {
            // Death();
            canMove = false;
            Invoke("Death", 0.4f);
            // PlayerEvents.PlayerDied();
            return false;
        }
    }

    public void Death(){
        PlayerEvents.PlayerDiedOnPlataformFall();
        PlayerEvents.PlayerDied();
    }

    public void Move(Vector3 newPosition)
    {
        if(IsValidJump(newPosition)){
            StartCoroutine(MoveToPosition(newPosition, "Jump"));
        } else {
            StartCoroutine(MoveAndDie(newPosition, "Jump"));
        }
    }

    private IEnumerator MoveToPosition(Vector3 newPosition, string animationName)
    {
        newPosition.y = -7.4f;
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
            animationHandler.PlayRandomJump(animationName, 0, t);

            transform.position = position;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
        animationHandler.Play("Idle");
        currentPlataform.Jumped();

        transform.position = newPosition;
        isMoving = false;
        // yield return new WaitForSeconds(1.5f);
        // animationHandler.PlayDeathAnimation(); //! Removido, responsabilidade externa

        if (moveQueue.Count > 0)
            moveQueue.Dequeue().Invoke();
    }

    private IEnumerator MoveAndDie(Vector3 newPosition, string animationName) //! classe duplicada, unificar com MoveToPosition
    {
        newPosition.y = -8.214834f;
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
            animationHandler.PlayRandomJump(animationName, 0, t);

            transform.position = position;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
        animationHandler.Play("DeathAnimation");

        transform.position = newPosition;
        isMoving = false;
        // yield return new WaitForSeconds(1.5f);
        // animationHandler.PlayDeathAnimation(); //! Removido, responsabilidade externa

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
        // CheckJumpPosition(newPosition); 
        // StartCoroutine(MoveToPosition(newPosition));
        Move(newPosition);
    }

    private void MoveRight(){
        if (!canMove || isMoving) 
        {
            if (isMoving) moveQueue.Enqueue(MoveRight);
            return;
        }
        Vector3 newPosition = currentPosition + Vector3.right * horizontalGridSize + Vector3.forward * verticalGridSize;
        // CheckJumpPosition(newPosition);
        // StartCoroutine(MoveToPosition(newPosition));
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
        // CheckJumpPosition(newPosition);
        // StartCoroutine(MoveToPosition(newPosition));
        Move(newPosition);
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

