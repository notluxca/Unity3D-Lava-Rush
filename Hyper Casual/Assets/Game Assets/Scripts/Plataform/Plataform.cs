using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Plataform : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] float distanceToFall;
    [SerializeField] Rigidbody rb;

    [Header("Shake Settings")]
    public Transform modelTransform;
    public float duration = 0.5f;
    public float strength = 0.3f;
    public int vibrato = 10;
    public float randomness = 90f;

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
            modelTransform.DOShakePosition(duration, strength, vibrato, randomness);
            StartCoroutine(Fall());

        }
    }

    public IEnumerator Fall(){
        yield return new WaitForSeconds(1);
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        rb.linearVelocity = new Vector3(0, -0.002f, 0);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}   
