using UnityEngine;

public class CollisionHandler : MonoBehaviour
{

    public static event System.Action collidedWithLava;

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Plataform":
                // collidedWithPlataform?.Invoke();
                break;
            case "Lava":
                collidedWithLava?.Invoke();
                break;
        }
    }
}
