using UnityEngine;

[System.Serializable]
public class EnemyStateData
{

    [field: Header("State")]
    [field: SerializeField] public EnemyPatrolData PatrolData { get; private set; }
    [field: SerializeField] public EnemyAttackData AttackData { get; private set; }
    [field: SerializeField] public EnemyGuardData GuardData { get; private set; }
    [field: SerializeField] public EnemyHitData HitData { get; private set; }

}
