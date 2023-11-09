using UnityEngine;

[System.Serializable]
public class EnemyGuardData
{
    [field: Header("방어 변수")]
    [field: SerializeField] public float guardInterval = 1.5f;  //가드 지속시간
    [field: SerializeField] public bool isGuarding { get; private set;} = false;

    public void SetIsGuarding(bool guard)
    {
        isGuarding = guard;
    }
}
