using System.Collections;
using UnityEngine;

public class BuffSkill : MonoBehaviour
{
    public GameObject attackBuffPrefab;
    private PlayerController playerController;

    private float attackPowerBuffAmount = 0.3f;
    private float buffDuration = 30f;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("Not found playerController");
        }
    }

    public void ActivateTime()
    {
        GameObject buffInstance = Instantiate(attackBuffPrefab, transform.position, Quaternion.identity);
        buffInstance.transform.SetParent(transform);

        StartCoroutine(ApplyAttackBuff());
    }

    private IEnumerator ApplyAttackBuff()
    {
        float originalAttackPower = playerController.currentAttackPower;

        playerController.currentAttackPower *= (1 + attackPowerBuffAmount);

        Debug.Log(playerController.currentAttackPower.ToString());

        yield return new WaitForSeconds(buffDuration);

        playerController.currentAttackPower = originalAttackPower;

        Debug.Log(playerController.currentAttackPower.ToString());
    }
}
