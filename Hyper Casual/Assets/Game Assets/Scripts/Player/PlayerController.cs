using System.Collections;
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

    //! outside responsibility
    int currentScore = 0;
    private bool isMoving = false;

    void Start()
    {
        Application.targetFrameRate = 60; 
        targetPosition = transform.position;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleKeyboardInput();
    }

    void HandleKeyboardInput()
    {
        if (!canMove || isMoving) return;

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
            moveDuration = 0.1f;
            return;
        }
        Vector3 newPosition = targetPosition + Vector3.forward * gridSize;
        CheckJumpPosition(newPosition);
        StartCoroutine(MoveToPosition(newPosition));
    }

    public void MoveDiagonalWithSwipe(Vector2 swipeDirection)
    {
        if (!canMove) return;
        if (isMoving)
        {
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
        RaycastHit hit;
        if (Physics.Raycast(position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        { 
            Debug.DrawRay(position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow, 2); 
            if (!hit.collider.gameObject.CompareTag("Plataform"))
            {
                canMove = false;
                Debug.Log("Movimento ilegal");
                StartCoroutine(FastLost());
            }
        }
    }

   public System.Collections.IEnumerator MoveToPosition(Vector3 destination)
{
    isMoving = true;
    Vector3 startPosition = transform.position;
    targetPosition = destination;
    float elapsedTime = 0;

    // Define a rotação inicial como "neutra" no início do movimento
    Quaternion startRotation = Quaternion.Euler(0, 0, 0);
    transform.rotation = startRotation; // Reseta para rotação padrão

    // Determina a direção horizontal
    Vector3 direction = (destination - startPosition).normalized;
    float tiltAngle = 0;

    if (direction.x > 0.5f) // Indo para a direita
    {
        tiltAngle = 15f; // Inclinação positiva para a direita
    }
    else if (direction.x < -0.5f) // Indo para a esquerda
    {
        tiltAngle = -15f; // Inclinação negativa para a esquerda
    }

    Quaternion targetRotation = Quaternion.Euler(0, tiltAngle * 3, tiltAngle * 2); // Inclinação

    // Iniciar a animação do pulo no início do movimento
    animator.Play("Jump", 0, 0); // Reproduz a animação do pulo do início

    while (elapsedTime < moveDuration)
    {
        elapsedTime += Time.deltaTime;
        float t = elapsedTime / moveDuration; // Normaliza o tempo entre 0 e 1

        // Smooth interpolation for position
        Vector3 position = Vector3.Lerp(startPosition, destination, t);

        // Ajusta o movimento vertical com base no arco do pulo
        position.y = Mathf.Sin(t * Mathf.PI) * jumpHeight + Mathf.Min(startPosition.y, destination.y);

        // Interpola a rotação para a inclinação
        transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

        // Sincroniza a animação com o progresso do movimento
        animator.Play("Jump", 0, t); // Avança a animação proporcionalmente

        transform.position = position;
        yield return null;
    }

    // Ao final do movimento, retorna para a rotação padrão (olhando para frente)
    transform.rotation = Quaternion.Euler(0, 0, 0);
    animator.Play("Idle", 0); // Retorna para a animação Idle

    // Garante a posição final e reinicia o estado
    transform.position = destination;
    isMoving = false;
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

    IEnumerator FastLost(){
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
