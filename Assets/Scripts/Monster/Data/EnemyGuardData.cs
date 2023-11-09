using UnityEngine;

[System.Serializable]
public class EnemyGuardData
{
    [field: Header("��� ����")]
    [field: SerializeField] public float guardInterval = 1.5f;  //���� ���ӽð�
    [field: SerializeField] public bool isGuarding { get; private set;} = false;

    public void SetIsGuarding(bool guard)
    {
        isGuarding = guard;
    }
}
