using UnityEngine;

[System.Serializable]
public class EnemyPatrolData
{
    [field: Header("순찰 변수")]
    [field: SerializeField] public float patrolRadius { get; private set; } = 5f;      //순찰 반경
    [field: SerializeField] public float patrolDuration  {get; private set;} = 5f;     //한 위치에 멈춰있을 시간
    [field: SerializeField] public float detectionRadius  {get; private set;} = 8f;    //플레이어 감지 거리
    [field: SerializeField] public float stopDistance  {get; private set;} = 2.5f;     //플레이어와 일정 거리 유지
}
