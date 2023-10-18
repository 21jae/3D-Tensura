using System.Collections;
using UnityEngine;

public class BuffSkill : MonoBehaviour, ISkill
{
    public GameObject attackBuffPrefab;
    private PlayerController playerController;

    private float attackPowerBuffAmount = 0.3f; //30% ���
    private float buffDuration = 3f;            //���� ���ӽð�

    private float originalAttackPower;          //�⺻ ATK ��
    private float increaseAttackPower;          //������ ATK ��

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
        //���� �� ���ݷ� ���
        originalAttackPower = playerController.playerStatManager.currentAttackPower;

        //������ ���� ������ ���
        increaseAttackPower = originalAttackPower * attackPowerBuffAmount;

        //���ݷ� ���� ����
        playerController.playerStatManager.ModifyAttackPower(increaseAttackPower);
        Debug.Log($" ATK : {playerController.playerStatManager.currentAttackPower}");

        //���� ���� ���ӽð�
        yield return new WaitForSeconds(buffDuration);

        //���ݷ� ���� ����
        playerController.playerStatManager.ModifyAttackPower(-increaseAttackPower);
        Debug.Log($" ATK : {playerController.playerStatManager.currentAttackPower}");
    }

}