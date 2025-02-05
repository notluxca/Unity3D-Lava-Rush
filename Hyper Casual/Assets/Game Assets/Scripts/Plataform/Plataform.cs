using UnityEngine;

public class Plataform : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] float distanceToFall;
    [SerializeField] Rigidbody rb;
    

    public static event System.Action OnPlataformJump;
    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if(transform.position.z + distanceToFall < mainCamera.transform.position.z){
            // Fall();
        }
    }

        /// <summary>
        /// Triggered when the player hits the platform.
        /// If the player hits the platform, it starts falling.
        /// </summary>
        /// <param name="other">The collision object</param>
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Player")){
            Debug.Log($"Player hit the platform {gameObject.name}");
            // GetComponent<Animator>().SetTrigger("Fall");
            OnPlataformJump?.Invoke();
        }
    }

    public void Fall(){
        rb.constraints = RigidbodyConstraints.None;
        rb.linearVelocity = new Vector3(0, -0.8f, 0);
    }
}   
