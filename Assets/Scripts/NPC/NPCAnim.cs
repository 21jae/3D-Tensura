using UnityEngine;

public class NPCAnim : MonoBehaviour
{
    [HideInInspector] public Animator animator;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void DialogNPC01Animation()
    {
        SoundManager.Instance.PlayNPCSound01();
        animator.SetBool("isTalking", true);
    }
    public void DialogNPC02Animation()
    {
        SoundManager.Instance.PlayNPCSound02();
        animator.SetBool("isTalking", true);
    }
}
