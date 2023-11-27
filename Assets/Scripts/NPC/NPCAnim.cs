using UnityEngine;

public class NPCAnim : MonoBehaviour
{
    [HideInInspector] public Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void DialogAnimation()
    {
        animator.SetBool("isTalking", true);
    }
}
