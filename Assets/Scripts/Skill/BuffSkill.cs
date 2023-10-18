using System.Collections;
using UnityEngine;

public class BuffSkill : MonoBehaviour, ISkill
{
    public GameObject attackBuffPrefab;
    private PlayerController playerController;

    private float attackPowerBuffAmount = 0.3f; //30% 상승
    private float buffDuration = 3f;            //버프 지속시간

    private float originalAttackPower;          //기본 ATK 값
    private float increaseAttackPower;          //증가된 ATK 값

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("Not found playerController");
        }
    }

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
        //버프 전 공격력 계산
        originalAttackPower = playerController.playerStatManager.currentAttackPower;

        //버프로 인한 증가량 계산
        increaseAttackPower = originalAttackPower * attackPowerBuffAmount;

        //공격력 증가 적용
        playerController.playerStatManager.ModifyAttackPower(increaseAttackPower);
        Debug.Log($" ATK : {playerController.playerStatManager.currentAttackPower}");

        //공격 버프 지속시간
        yield return new WaitForSeconds(buffDuration);

        //공격력 원상 복구
        playerController.playerStatManager.ModifyAttackPower(-increaseAttackPower);
        Debug.Log($" ATK : {playerController.playerStatManager.currentAttackPower}");
    }

}