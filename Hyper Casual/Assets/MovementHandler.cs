using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] private bool canMove = true;
    [SerializeField] private float moveDuration = 0.2f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private Animator animator;
    AnimationHandler animationHandler;
    
    public static event System.Action OnPlayerMove;
    public static event System.Action OnPlayerFirstMove;

    private Queue<System.Action> moveQueue = new Queue<System.Action>();

    private Vector3 targetPosition;
    
    private bool isMoving = false;
    private float speedUpRate;

    private float horizontalGridSize;
    private float verticalGridSize;
    

    public void Initialize()
    {
        horizontalGridSize = GameInfo.Instance.horizontalGridSize;
        verticalGridSize = GameInfo.Instance.verticalGridSize;
        targetPosition = transform.position;
        animationHandler = GetComponent<AnimationHandler>();
    }

    public void MoveFront()
    {
        if (!canMove || isMoving) 
        {
            if (isMoving) moveQueue.Enqueue(MoveFront);
            return;
        }

        Vector3 newPosition = targetPosition + Vector3.forward * verticalGridSize;
        CheckJumpPosition(newPosition);
        StartCoroutine(MoveToPosition(newPosition));
    }

    public void MoveDiagonal(Vector2 swipeDirection)
    {
        if (!isMoving)
        {
            Vector3 direction = new Vector3(swipeDirection.x, 0, 1);
            Vector3 newPosition = targetPosition + direction;
            StartCoroutine(MoveToPosition(newPosition));
        }
    }

    public void MoveDiagonalWithSwipe(Vector2 swipeDirection)
    {
        if (!canMove || isMoving)
        {
            if (isMoving) moveQueue.Enqueue(() => MoveDiagonalWithSwipe(swipeDirection));
            return;
        }

        Vector3 horizontal = swipeDirection.x > 0 ? Vector3.right : Vector3.left;
        Vector3 newPosition = targetPosition + horizontal * horizontalGridSize + Vector3.forward * verticalGridSize;

        CheckJumpPosition(newPosition);
        StartCoroutine(MoveToPosition(newPosition));
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
            Debug.Log("Pulou no vazio");
            canMove = false;
            StartCoroutine(FastLost());
        }
    }

        /// <summary>
        /// Move the player to the given position.
        /// </summary>
        /// <param name="destination">The destination position.</param>
        /// <remarks>
        /// This function will move the player to the given position over a set duration.
        /// It will also play a jump animation and tilt the player object slightly.
        /// </remarks>
    private IEnumerator MoveToPosition(Vector3 destination)
    {
        if (OnPlayerFirstMove != null)
        {
            OnPlayerFirstMove.Invoke();
            OnPlayerFirstMove = null;
        }

        OnPlayerMove?.Invoke();
        isMoving = true;
        Vector3 startPosition = transform.position;
        targetPosition = destination;
        float elapsedTime = 0;
        Quaternion startRotation = Quaternion.Euler(0, 0, 0);
        transform.rotation = startRotation;

        Vector3 direction = (destination - startPosition).normalized;
        float tiltAngle = direction.x > 0.5f ? 15f : direction.x < -0.5f ? -15f : 0f;

        Quaternion targetRotation = Quaternion.Euler(0, tiltAngle * 3, tiltAngle * 2);
        //animator.Play("Jump", 0, 0);

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;

            Vector3 position = Vector3.Lerp(startPosition, destination, t);
            position.y = Mathf.Sin(t * Mathf.PI) * jumpHeight + Mathf.Min(startPosition.y, destination.y);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            animationHandler.PlayRandomJump("Jump 0", 0, t);

            transform.position = position;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
        animationHandler.Play("Idle");
        transform.position = destination;
        isMoving = false;

        if (moveQueue.Count > 0)
            moveQueue.Dequeue().Invoke();
    }

    private IEnumerator FastLost()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Fast lost called");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void MakeFaster()
    {
        if (moveDuration > 0.25f)
            moveDuration -= speedUpRate * Time.deltaTime;
        moveDuration = Mathf.Clamp(moveDuration, 0.25f, float.MaxValue);
    }

}

