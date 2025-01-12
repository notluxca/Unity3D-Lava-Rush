using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Main Player Settings")]
    [SerializeField] public float moveDuration = 0.2f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private bool canMove = true;

    public static event System.Action OnPlayerMove;

    private float verticalGridSize;
    private float horizontalGridSize;
    private Vector3 targetPosition;
    private Animator animator;
    private Queue<System.Action> moveQueue = new Queue<System.Action>();
    private bool isMoving = false;
    

    private void Start()
    {
        Application.targetFrameRate = 60;
        targetPosition = transform.position;
        animator = GetComponentInChildren<Animator>();
        horizontalGridSize = GameInfo.Instance.horizontalGridSize;
        verticalGridSize = GameInfo.Instance.verticalGridSize;
    }

    private void Update()
    {
        HandleKeyboardInput();
    }

    private void HandleKeyboardInput()
    {
        if (!canMove) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            MoveFront();
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            MoveDiagonalWithSwipe(Vector2.one);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            MoveDiagonalWithSwipe(new Vector2(-1, 1));
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
        }
        else
        {
            Debug.Log("Pulou no vazio");
            canMove = false;
            StartCoroutine(FastLost());
        }
    }

    public IEnumerator MoveToPosition(Vector3 destination)
    {
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
        animator.Play("Jump", 0, 0);

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;

            Vector3 position = Vector3.Lerp(startPosition, destination, t);
            position.y = Mathf.Sin(t * Mathf.PI) * jumpHeight + Mathf.Min(startPosition.y, destination.y);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            animator.Play("Jump", 0, t);

            transform.position = position;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, 0);
        animator.Play("Idle", 0);
        transform.position = destination;
        isMoving = false;

        if (moveQueue.Count > 0)
            moveQueue.Dequeue().Invoke();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Lava"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (other.collider.CompareTag("Plataform"))
        {
            Debug.Log($"Collision Detected , other: {other.gameObject.name}");
            FindFirstObjectByType<ScoreManager>().AddScore(1);
        }
    }

    private IEnumerator FastLost()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MakeFaster(float multiplier)
    {
        moveDuration -= multiplier;
    }
}

