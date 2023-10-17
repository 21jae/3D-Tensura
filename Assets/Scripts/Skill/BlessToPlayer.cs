using UnityEngine;

public class BlessToPlayer : MonoBehaviour
{
    private Animator blessAnim;

    private void Awake()
    {
        blessAnim = GetComponent<Animator>();
    }

    public void BlessTrigger()
    {
        blessAnim.SetTrigger("Bless");
    }

}
