using UnityEngine;

[System.Serializable]
public class EnemyAttackData
{
    [field: Header("공격 프리팹")]
    [field: SerializeField] public GameObject attackSlashPrefab { get; set; }

    [field: Header("공격 변수")]
    [field: SerializeField] public float attackInterval { get; private set; } = 1.5f;    //공격 간격
    [field: SerializeField] public bool isAttacking { get; private set; } = false;

    public void SetIsAttack(bool attack)
    {
        isAttacking = attack;
    }

}
