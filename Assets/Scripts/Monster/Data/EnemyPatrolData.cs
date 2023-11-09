using UnityEngine;

[System.Serializable]
public class EnemyPatrolData
{
    [field: Header("���� ����")]
    [field: SerializeField] public float patrolRadius { get; private set; } = 5f;      //���� �ݰ�
    [field: SerializeField] public float patrolDuration  {get; private set;} = 5f;     //�� ��ġ�� �������� �ð�
    [field: SerializeField] public float detectionRadius  {get; private set;} = 8f;    //�÷��̾� ���� �Ÿ�
    [field: SerializeField] public float stopDistance  {get; private set;} = 2.5f;     //�÷��̾�� ���� �Ÿ� ����
}
