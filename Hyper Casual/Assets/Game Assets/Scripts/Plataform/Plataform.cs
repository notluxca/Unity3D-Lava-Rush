using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Linq;


public class Platform : MonoBehaviour, IPlatform
{
    [SerializeField] Camera mainCamera;
    [SerializeField] float distanceToFall;
    [SerializeField] Rigidbody rb;
    [SerializeField] float timeToFall = 0.5f;
    [SerializeField] float timeToKill;

    // private bool PlayerOnPlatform = false;

    [Header("Shake Settings")]
    public Transform modelTransform;
    public float duration = 0.5f;
    public float strength = 0.3f;
    public int vibrato = 10;
    public float randomness = 90f;

    public static event System.Action OnPlatformJump;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if(transform.position.z + distanceToFall < mainCamera.transform.position.z)
        {
            // Fall();
        }
    }

    public void Jumped()
    {
        // PlayerOnPlatform = true;
        OnPlatformJump?.Invoke();
        modelTransform.DOShakePosition(duration, strength, vibrato, randomness);
        StartCoroutine(Fall());
    }

    private void OnCollisionExit(Collision other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
        // PlayerOnPlatform = false;
        }
    }

    private IEnumerator Fall()
    {
        Vector3 startPosition = transform.position;
        StartCoroutine(CheckPlayerDeath(startPosition));
        yield return new WaitForSeconds(timeToFall);
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        rb.linearVelocity = new Vector3(0, -0.002f, 0);
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }

    private IEnumerator CheckPlayerDeath(Vector3 position)
    {
        yield return new WaitForSeconds(timeToKill);
        if (Physics.Raycast(position, Vector3.up, out RaycastHit hit, 6.5f, LayerMask.GetMask("Player")))
        {
            PlayerEvents.PlayerDiedOnPlataformFall();
            PlayerEvents.PlayerDied();
        }
    }

    private void OnDestroy()
    {
        modelTransform?.DOKill();
    }
} 
