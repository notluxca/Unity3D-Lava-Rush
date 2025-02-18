using UnityEngine;

public class ParticlesHandler : MonoBehaviour
{
    public ParticleSystem FootStepParticles;
    public ParticleSystem LavaBurnParticles;

    private void OnEnable() {
        PlayerEvents.OnPlayerCollidedWithPlatform += PlayFootstepParticles;
        PlayerEvents.onPlayerDiedByPlataformFall += PlayLavaBurnParticles;
    }

    private void OnDisable() {
        PlayerEvents.OnPlayerCollidedWithPlatform -= PlayFootstepParticles;
        PlayerEvents.onPlayerDiedByPlataformFall -= PlayLavaBurnParticles;
    }
    
    public void PlayFootstepParticles()
    {
        if (FootStepParticles != null) {
            FootStepParticles.Play();     
        } else {
            // Debug.LogError("FootStepParticles não foi atribuído no inspetor!");
        }
    }

    public void PlayLavaBurnParticles()
    {
        if (LavaBurnParticles != null) {
            LavaBurnParticles.Play();
        } else {
            // Debug.LogError("LavaBurnParticles não foi atribuído no inspetor!");
        }
    }

    
}
