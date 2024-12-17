using UnityEngine;

public class Plataform : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] float distanceToFall;
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z + distanceToFall < mainCamera.transform.position.z){
            Destroy(this.gameObject);
        }
    }
}
