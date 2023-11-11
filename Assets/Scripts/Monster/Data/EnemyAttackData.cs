using UnityEngine;

[System.Serializable]
public class EnemyAttackData
{
    [field: Header("���� ������")]
    [field: SerializeField] public GameObject attackSlashPrefab { get; set; }

    [field: Header("���� ����")]
    [field: SerializeField] public float attackInterval { get; private set; } = 1.5f;    //���� ����
    [field: SerializeField] public bool isAttacking { get; private set; } = false;

    public void SetIsAttack(bool attack)
    {
        isAttacking = attack;
    }

}
