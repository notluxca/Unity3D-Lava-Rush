using UnityEngine;

public class ParticlesHandler : MonoBehaviour
{
    private ParticleSystem particleSystem;

    private void Awake() {
        particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void OnEnable() {
        CollisionHandler.collidedWithPlataform += PlayFootstepParticles;
    }

    private void OnDisable() {
        CollisionHandler.collidedWithPlataform -= PlayFootstepParticles;
    }
    
    public void PlayFootstepParticles()
    {
        particleSystem.Play();
    }
    
}
