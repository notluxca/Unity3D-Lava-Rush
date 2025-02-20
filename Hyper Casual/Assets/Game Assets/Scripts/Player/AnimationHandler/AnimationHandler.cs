using System.Collections;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private Animator animator;
    public GameObject animationGameobject;

    private void Start() {
        animator = GetComponentInChildren<Animator>();
        PlayerEvents.onPlayerDiedByPlataformFall += PlayDeathAnimation;
        PlayerEvents.OnCharacterModelChanged += RestartAnimator;
    }

    private void OnDisable() {
        PlayerEvents.onPlayerDiedByPlataformFall -= PlayDeathAnimation;
        PlayerEvents.OnCharacterModelChanged -= RestartAnimator;
    }

    public void Play(string animationName){
        animator.Play(animationName,0);
    }

    public void PlayRandomJump(string stateName, [UnityEngine.Internal.DefaultValue("-1")] int layer, [UnityEngine.Internal.DefaultValue("float.NegativeInfinity")] float normalizedTime){
        // Debug.Log(normalizedTime);
        animator.Play(stateName, layer, normalizedTime);
    }
    
    public void PlayDeathAnimation(){
        animator.Play("DeathAnimation", 0);
    }

    public void RestartAnimator(){
        Debug.Log("Pasei pelo rebind");
        animator.Rebind();
        animator.Update(0f);
        // animator.CrossFade("Idle", 0.1f);
        Invoke("RestartCharacter", 0.1f);
    }
    
    //TODO: we could use a coroutine to wait for the animation to finish while mainting its reference on a object and stopping the restart if another change happens
    private void RestartCharacter(){
        animationGameobject.SetActive(false);
        animationGameobject.SetActive(true);
    }

    
}
