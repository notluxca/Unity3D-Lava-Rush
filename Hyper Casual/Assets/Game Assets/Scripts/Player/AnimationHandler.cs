using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private Animator animator;

    private void Start() {
        animator = GetComponentInChildren<Animator>();
        PlayerEvents.onPlayerDiedByPlataformFall += PlayDeathAnimation;
    }

    public void Play(string animationName){
        animator.Play(animationName,0);
    }

    public void PlayRandomJump(string stateName, [UnityEngine.Internal.DefaultValue("-1")] int layer, [UnityEngine.Internal.DefaultValue("float.NegativeInfinity")] float normalizedTime){
        // Debug.Log(normalizedTime);
        animator.Play(stateName, layer, normalizedTime);
    }
    
    public void PlayDeathAnimation(){
        // animator.Play("DeathAnimation", 0, 1);
    }
}
