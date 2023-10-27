using System.Collections;
using UnityEngine;

public class BuffSkill : MonoBehaviour, ISkill
{
    public GameObject attackBuffPrefab;
    private PlayerController playerController;

    private float attackPowerBuffPercentage = 0.6f;
    private float buffDuration = 20f;
    private float originalAttackPower;
    private float increaseAttackPower;

    #region 초기화
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("Not found playerController");
        }
    }
    #endregion

    #region 스킬 활성화 및 버프 로직
    public void ActivateSkill()
    {
        ActivateTime();
    }

    private void ActivateTime()
    {
        GameObject buffInstance = Instantiate(attackBuffPrefab, playerController.transform.position, Quaternion.identity);
        buffInstance.transform.SetParent(playerController.transform);

        StartCoroutine(ApplyAttackBuff());
    }

    private IEnumerator ApplyAttackBuff()
    {
        CalculateAndApplyAttackPower();

        yield return new WaitForSeconds(buffDuration);

        ResetAttackPowerToOriginal();
    }

    private void CalculateAndApplyAttackPower()
    {
        originalAttackPower = playerController.playerStatManager.currentAttackPower;
        increaseAttackPower = originalAttackPower * attackPowerBuffPercentage;
        playerController.playerStatManager.ModifyAttackPower(increaseAttackPower);
        Debug.Log($" ATK : {playerController.playerStatManager.currentAttackPower}");
    }

    private void ResetAttackPowerToOriginal()
    {
        playerController.playerStatManager.ModifyAttackPower(-increaseAttackPower);
        Debug.Log($" ATK : {playerController.playerStatManager.currentAttackPower}");
    }
    #endregion
}