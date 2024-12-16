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

    void Update()
    {
        if (!isMoving)
        {
            // Keyboard controls
            if (Input.GetKeyDown(KeyCode.W)) Move(Vector3.forward); // Forward
            if (Input.GetKeyDown(KeyCode.A)) MoveDiagonal(Vector3.left, Vector3.forward); // Diagonal left-up
            if (Input.GetKeyDown(KeyCode.D)) MoveDiagonal(Vector3.right, Vector3.forward); // Diagonal right-up

            // Detect swipes
            Swipe();
        }
    }

    //inside class


public void Swipe()
{
     if(Input.touches.Length > 0)
     {
	     Touch t = Input.GetTouch(0);
	     if(t.phase == TouchPhase.Began)
	     {
	          //save began touch 2d point
		     firstPressPos = new Vector2(t.position.x,t.position.y);
	     }
	     if(t.phase == TouchPhase.Ended)
	     {
              //save ended touch 2d point
		     secondPressPos = new Vector2(t.position.x,t.position.y);
		     				
              //create vector from the two points
		     currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
				
		     //normalize the 2d vector
		     currentSwipe.Normalize();

		     //swipe upwards
		     if(currentSwipe.y > 0  currentSwipe.x > -0.5f  currentSwipe.x < 0.5f)
		     {
			     Debug.Log("up swipe");
		     }
		     //swipe down
		     if(currentSwipe.y < 0  currentSwipe.x > -0.5f  currentSwipe.x < 0.5f)
		     {
			     Debug.Log("down swipe");
		     }
		     //swipe left
		     if(currentSwipe.x < 0  currentSwipe.y > -0.5f  currentSwipe.y < 0.5f)
		     {
			     Debug.Log("left swipe");
		     }
		     //swipe right
		     if(currentSwipe.x > 0  currentSwipe.y > -0.5f  currentSwipe.y < 0.5f)
		     {
			     Debug.Log("right swipe");
		     }
	     }
     }
}

    void HandleTouchOrSwipe()
    {
        Vector2 swipe = endTouchPosition - startTouchPosition;

        if (!swipeDetected && swipe.magnitude < minSwipeDistance)
        {
            // Treat as a simple tap
            Move(Vector3.forward);
        }
        else if (swipeDetected)
        {
            // Normalize swipe direction
            swipe.Normalize();

            if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
            {
                if (swipe.x > 0) MoveDiagonal(Vector3.right, Vector3.forward); // Swipe right-up
                else MoveDiagonal(Vector3.left, Vector3.forward); // Swipe left-up
            }
            else if (swipe.y > 0) // Swipe up
            {
                Move(Vector3.forward);
            }
        }
    }

    void Move(Vector3 direction)
    {
        Vector3 newPosition = targetPosition + direction * gridSize;
        animator.SetTrigger("Jump");
        StartCoroutine(MoveToPosition(newPosition));
    }

    void MoveDiagonal(Vector3 horizontal, Vector3 vertical)
    {
        Vector3 newPosition = targetPosition + (horizontal + vertical) * gridSize;
        animator.SetTrigger("Jump");
        StartCoroutine(MoveToPosition(newPosition));
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
