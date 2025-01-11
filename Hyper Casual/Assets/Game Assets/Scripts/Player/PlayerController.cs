using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Main Player Settings")]
    [SerializeField] float moveDuration = 0.2f; // Time for one move
    [SerializeField] float jumpHeight = 1.0f;  // Maximum jump height
    [SerializeField] bool canMove = true;
    //* public static event of player jump

    public static event System.Action OnPlayerMove; // notify everyone of new PlayerMov


    
    
    float verticalGridSize;    // Distance between grid positions
    float horizontalGridSize;    // Distance between grid positions


    //* Private Data
    private Vector3 targetPosition;
    private Animator animator;
    private Queue<System.Action> moveQueue = new Queue<System.Action>();
    private bool isMoving = false;

    //! outside responsibility
    int currentScore = 0;

    void Start()
    {
        Application.targetFrameRate = 60; 
        targetPosition = transform.position;
        animator = GetComponentInChildren<Animator>();
        
        horizontalGridSize = GameInfo.Instance.horizontalGridSize;
        verticalGridSize = GameInfo.Instance.verticalGridSize;
    }

    void Update()
    {
        HandleKeyboardInput();
    }

    void HandleKeyboardInput()
    {
        if (!canMove) return;

        if (Input.GetKeyDown(KeyCode.UpArrow)) // Move para frente
        {
            MoveFront();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow)) // Move na diagonal direita
        {
            MoveDiagonalWithSwipe(new Vector2(1, 1)); // Simula swipe diagonal
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) // Move na diagonal esquerda
        {
            MoveDiagonalWithSwipe(new Vector2(-1, 1)); // Simula swipe diagonal
        }
    }

    public void MoveFront()
    {
        if (!canMove) return;

        if (isMoving)
        {
            moveQueue.Enqueue(() => MoveFront());
            return;
        }

        Vector3 newPosition = targetPosition + Vector3.forward * verticalGridSize;
        CheckJumpPosition(newPosition);
        StartCoroutine(MoveToPosition(newPosition));
    }

    public void MoveDiagonalWithSwipe(Vector2 swipeDirection)
    {
        if (!canMove) return;

        if (isMoving)
        {
            moveQueue.Enqueue(() => MoveDiagonalWithSwipe(swipeDirection));
            return;
        }

        var horizontal = swipeDirection.x > 0 ? Vector3.right : Vector3.left;
        var vertical = Vector3.forward * verticalGridSize;
        var newPosition = targetPosition + horizontal * horizontalGridSize + vertical;

        CheckJumpPosition(newPosition);
        StartCoroutine(MoveToPosition(newPosition));
    }

    void CheckJumpPosition(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, transform.TransformDirection(Vector3.down), out hit, 20f))
        { 
            Debug.DrawRay(position, transform.TransformDirection(Vector3.down) * hit.distance, Color.green, 10); 
            if (!hit.collider.gameObject.CompareTag("Plataform"))
            {
                canMove = false;
                Debug.Log("Movimento ilegal");
                StartCoroutine(FastLost());
            }
        }
        else
        {
            Debug.Log("Pulou no vazio");
            canMove = false;
            StartCoroutine(FastLost());
        }
    }

    /// <summary>
    /// Move to a given position in a smooth animation
    /// </summary>
    /// <param name="destination">The position to move to</param>
    /// <returns>An IEnumerator to be used in a coroutine</returns>
    public IEnumerator MoveToPosition(Vector3 destination)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        targetPosition = destination;
        float elapsedTime = 0;

        Quaternion startRotation = Quaternion.Euler(0, 0, 0);
        transform.rotation = startRotation;

        Vector3 direction = (destination - startPosition).normalized;
        float tiltAngle = 0;

        if (direction.x > 0.5f)
        {
            tiltAngle = 15f;
        }
        else if (direction.x < -0.5f)
        {
            tiltAngle = -15f;
        }

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
        {
            moveQueue.Dequeue().Invoke();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Lava"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (other.gameObject.CompareTag("Plataform"))
        {
            currentScore += 1;
            UIManager.Instance.UpdateScoreUI(currentScore);
        }
    }

    IEnumerator FastLost()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
