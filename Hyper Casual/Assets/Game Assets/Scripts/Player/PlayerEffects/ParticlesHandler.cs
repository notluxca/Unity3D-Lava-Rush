using System;
using UnityEngine;

public class ParticlesHandler : MonoBehaviour
{
    public ParticleSystem FootStepParticles;
    public ParticleSystem LavaBurnParticles;

    private void OnEnable()
    {
        PlayerEvents.OnPlayerCollidedWithPlatform += PlayFootstepParticles;
        PlayerEvents.onPlayerDiedByPlataformFall += PlayLavaBurnParticles;
        PlayerEvents.onPlayerDied += PlayLavaBurnParticles;
        PlayerEvents.onPlayerRevived += TurnOffParticles;
    }

    private void OnDisable()
    {
        PlayerEvents.OnPlayerCollidedWithPlatform -= PlayFootstepParticles;
        PlayerEvents.onPlayerDiedByPlataformFall -= PlayLavaBurnParticles;
        PlayerEvents.onPlayerDied -= PlayLavaBurnParticles;
        PlayerEvents.onPlayerRevived -= TurnOffParticles;
    }

    private void TurnOffParticles()
    {
        Debug.Log("Turn off particles");
        LavaBurnParticles.Stop();
    }

    public void PlayFootstepParticles()
    {
        if (FootStepParticles != null)
        {
            FootStepParticles.Play();
        }
    }

    public void PlayLavaBurnParticles()
    {
        Debug.Log("Paying particles");
        if (LavaBurnParticles != null) LavaBurnParticles.Play();
    }


}
