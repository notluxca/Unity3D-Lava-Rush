using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Linq;

public class Plataform : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] float distanceToFall;
    [SerializeField] Rigidbody rb;
    [SerializeField] float timeToFall = 0.5f;
    [SerializeField] float timeToKill;

    [SerializeField] private bool PlayerOnPlataform = false;

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

    public void Jumped(){
        PlayerOnPlataform = true;
        OnPlataformJump?.Invoke();
        modelTransform.DOShakePosition(duration, strength, vibrato, randomness);
        StartCoroutine(Fall());
    }

    private void OnCollisionEnter(Collision other) {
        // if(other.gameObject.CompareTag("Player")){
        //     // Debug.Log($"Player hit the platform {gameObject.name}");
        //     PlayerOnPlataform = true;
        //     OnPlataformJump?.Invoke();
        //     modelTransform.DOShakePosition(duration, strength, vibrato, randomness);
        //     StartCoroutine(Fall());
        // }
    }

    private void OnCollisionExit(Collision other) {
        if(other.gameObject.CompareTag("Player")){
            PlayerOnPlataform = true;
        }
    }

    public IEnumerator Fall(){
        Vector3 startPosition = transform.position;
        StartCoroutine(CheckPlayerDeath(startPosition));
        yield return new WaitForSeconds(timeToFall);
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY; //* Aplica velocidade para baixo
        rb.linearVelocity = new Vector3(0, -0.002f, 0);
        yield return new WaitForSeconds(1);        
        Destroy(gameObject);
    }



    // Raycast upwards to check if player is still on the plataform
    public IEnumerator CheckPlayerDeath(Vector3 position){
        yield return new WaitForSeconds(timeToKill);
        RaycastHit hit;
        Debug.DrawRay(position, Vector3.up * 6.5f, Color.green, 1);
        if(Physics.Raycast(position, Vector3.up, out hit, 6.5f, LayerMask.GetMask("Player"))){
                PlayerEvents.PlayerDiedOnPlataformFall();
                PlayerEvents.PlayerDied();
        }
    }

    private void OnDestroy()
    {
        modelTransform?.DOKill();
    }
}

