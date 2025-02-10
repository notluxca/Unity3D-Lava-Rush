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

        /// <summary>
        /// Triggered when the player hits the platform.
        /// If the player hits the platform, it starts falling.
        /// </summary>
        /// <param name="other">The collision object</param>
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.CompareTag("Player")){
            // Debug.Log($"Player hit the platform {gameObject.name}");
            // GetComponent<Animator>().SetTrigger("Fall");
            PlayerOnPlataform = true;
            OnPlataformJump?.Invoke();
            modelTransform.DOShakePosition(duration, strength, vibrato, randomness);
            StartCoroutine(Fall());

        }
    }

    private void OnCollisionExit(Collision other) {
        if(other.gameObject.CompareTag("Player")){
            PlayerOnPlataform = true;
        }
    }

    public IEnumerator Fall(){
        Vector3 startPosition = transform.position;
        
        yield return new WaitForSeconds(timeToFall);
        CheckPlayerOnPlataform(startPosition);
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY; //* Aplica velocidade para baixo
        rb.linearVelocity = new Vector3(0, -0.002f, 0);
        
        // if(PlayerOnPlataform){
        //         // Debug.Log(message: "PLAYER HIT");
        //         // PlayerEvents.PlayerDiedOnPlataformFall();
        //         // PlayerEvents.PlayerDied();
        // }
        yield return new WaitForSeconds(1);        
        Destroy(gameObject);
    }


    // Overlap a box collider to check if player was still on the plataform on the moment of Plataform Fall
    public void CheckPlayerOnPlataform(Vector3 position){
            // DebugExtension.DrawBounds(new Bounds(position, new Vector3(0.5f,0.5f,0.5f)), Color.red);
            Collider[] hit = Physics.OverlapBox(position, new Vector2(0.5f,0.5f), Quaternion.identity, LayerMask.GetMask("Player"));
            if (hit != null)
            {
                
                Debug.Log("Player detectado durante a queda");
            } else {
                
            }
    }

    void OnDrawGizmos()
    {
        DebugExtension.DrawBounds(new Bounds(transform.position, new Vector3(0.5f, 0.5f, 0.5f)), Color.red);
    }

        


}

