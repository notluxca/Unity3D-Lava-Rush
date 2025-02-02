using UnityEngine;

public class CollisionHandler : MonoBehaviour
{

    public static event System.Action collidedWithLava;
    public static event System.Action collidedWithPlataform;

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Plataform")) collidedWithPlataform?.Invoke();
        else if (other.collider.CompareTag("Lava")) collidedWithLava?.Invoke();     
    }
}
