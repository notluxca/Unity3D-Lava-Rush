using UnityEngine;

public class TransitionScreen : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        animator.Play("BlackscreenTransition", 0);
    }

    public void FadeOut()
    {
        animator.Play("FadeOut", 0);
    }
}
