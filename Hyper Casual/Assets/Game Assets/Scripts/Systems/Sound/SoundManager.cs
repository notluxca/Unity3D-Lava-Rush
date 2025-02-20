using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip plataformSound;


    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = plataformSound;
    }

    private void OnEnable() {
        PlayerEvents.OnPlayerCollidedWithPlatform += PlayPlataformSound;    
    }
    private void OnDisable() {
        PlayerEvents.OnPlayerCollidedWithPlatform -= PlayPlataformSound;
    }

    public void PlayPlataformSound(){
        Debug.Log("Som solicitado");
        audioSource.Play();
    }
}
