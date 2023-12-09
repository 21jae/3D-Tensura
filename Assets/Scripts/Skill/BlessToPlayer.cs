using System.Collections;
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
        StartCoroutine(WaitBlessSound());
    }

    private IEnumerator WaitBlessSound()
    {
        yield return CoroutineHelper.WaitForSeconds(1f);
        SoundManager.Instance.PlayBlessingSound();
    }

}
