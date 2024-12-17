using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    [Header("Main Player Settings")]
    [SerializeField] float moveDuration = 0.2f; // Time for one move
    [SerializeField] float jumpHeight = 1.0f;  // Maximum jump height
    [SerializeField] bool canMove = true;
    
    [Header("Mov Grid Settings")]
    [SerializeField] float gridSize = 1.0f;    // Distance between grid positions
    
    //* Private Data
    private Vector3 targetPosition;
    private Animator animator;


    //! outside responsability
    int currentScore = 0;
    private bool isMoving = false;
    

    

    void Start()
    {
        Application.targetFrameRate = 120; 
        targetPosition = transform.position;
        animator = GetComponent<Animator>();
    }


    public void MoveFront()
    {
        if(!canMove) return;
        if(isMoving == true) {
            moveDuration = 0.1f;
            return;
        }
        Vector3 newPosition = targetPosition + Vector3.forward * gridSize;
        CheckJumpPosition(newPosition);
        StartCoroutine(MoveToPosition(newPosition));
    }

    public void MoveDiagonalWithSwipe(Vector2 swipeDirection)
    {
        if(!canMove) return;
        if(isMoving == true) {
            moveDuration = 0.1f;
            return;
        }
        Vector3 horizontal = swipeDirection.x > 0 ? Vector3.right : Vector3.left;
        Vector3 vertical = Vector3.forward;
        Vector3 newPosition = targetPosition + (horizontal + vertical) * gridSize;
        CheckJumpPosition(newPosition);
        StartCoroutine(MoveToPosition(newPosition));

    }
    void CheckJumpPosition(Vector3 position)
    {
        // Adjust logic to check grid-based movement validity
        // Raycast new position
        // check plataform or lava -- cancel r liberate next movment
        RaycastHit hit;
        if (Physics.Raycast(position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        { 
            Debug.DrawRay(position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow, 2); 
            if(!hit.collider.gameObject.CompareTag("Plataform")){
                canMove = false;
            }
        }
        
    }

    System.Collections.IEnumerator MoveToPosition(Vector3 destination)
    {
        
        isMoving = true;
        Vector3 startPosition = transform.position;
        targetPosition = destination;
        float elapsedTime = 0;

        // yield return new WaitForSeconds(0.10f);
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
        moveDuration = 0.3f;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Lava"))
        {
            // ebug.Break(); pause game on editor
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (other.gameObject.CompareTag("Plataform"))
        {
            currentScore += 1;
            UIManager.Instance.UpdateScoreUI(currentScore);
        }
    }
}
