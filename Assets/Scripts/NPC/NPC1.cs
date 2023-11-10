using UnityEngine;

public class NPC1 : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private GameObject Mask;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();   
    }

    public void TalkingStart()
    {
        animator.SetBool("isTalking", true);
    }

    public void TalkingEnd()
    {
        animator.SetBool("isTalking", false);
    }

    public void OffMask()
    {
        Mask.SetActive(false);
    }
}
