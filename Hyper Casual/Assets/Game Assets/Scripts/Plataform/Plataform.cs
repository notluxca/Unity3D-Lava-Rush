using UnityEngine;

public class Plataform : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] float distanceToFall;
    [SerializeField] Rigidbody rb;
    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if(transform.position.z + distanceToFall < mainCamera.transform.position.z){
            rb.constraints = RigidbodyConstraints.None;
            rb.linearVelocity = new Vector3(0, -0.8f, 0);
        }
    }
}   
