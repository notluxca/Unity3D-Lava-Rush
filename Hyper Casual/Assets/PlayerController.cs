using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float minSwipeDistance = 200f; // Minimum swipe distance in pixels
    public float moveDuration = 0.2f; // Time for one move
    public float jumpHeight = 1.0f;  // Maximum jump height
    public float gridSize = 1.0f;    // Distance between grid positions
    private Vector3 targetPosition;
    private bool isMoving = false;

    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    private bool swipeDetected = false;
    private Animator animator;

    void Start()
    {
        Application.targetFrameRate = 120; 
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
    }


    public void MoveFront()
    {
        Vector3 newPosition = targetPosition + Vector3.forward * gridSize;
        StartCoroutine(MoveToPosition(newPosition));
    }

    public void MoveDiagonalWithSwipe(Vector2 swipeDirection)
            {
                Vector3 horizontal = swipeDirection.x > 0 ? Vector3.right : Vector3.left;
                Vector3 vertical = Vector3.forward;

                Vector3 newPosition = targetPosition + (horizontal + vertical) * gridSize;
                    StartCoroutine(MoveToPosition(newPosition));

                if (IsValidPosition(newPosition))
                {
                }
                else
                {
                    Debug.Log("Invalid diagonal position: " + newPosition);
                }
            }
    bool IsValidPosition(Vector3 position)
    {
        // Adjust logic to check grid-based movement validity
        return Mathf.Approximately(position.y, 0.0f);
    }

    System.Collections.IEnumerator MoveToPosition(Vector3 destination)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        targetPosition = destination;
        float elapsedTime = 0;

        yield return new WaitForSeconds(0.10f);
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;

            // Smooth interpolation for position
            Vector3 position = Vector3.Lerp(startPosition, destination, t);

            // Adjust vertical jump arc to ensure smooth motion
            position.y = Mathf.Sin(t * Mathf.PI) * jumpHeight + Mathf.Min(startPosition.y, destination.y);

            transform.position = position;
            yield return null;
        }

        transform.position = destination;
        isMoving = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Lava"))
        {
            Debug.Break();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
