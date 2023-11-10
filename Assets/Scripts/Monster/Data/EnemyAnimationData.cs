using UnityEngine;

[System.Serializable]
public class EnemyAnimationData
{
    [SerializeField] private string moveParameterName = "Speed";
    [SerializeField] private string attackParameterName = "isAttacking";
    [SerializeField] private string guardParamterName = "isGuarding";
    [SerializeField] private string deathParameterName = "Death";

    public int MoveParameterName { get; private set; }
    public int AttackParameterName { get; private set; }
    public int GuardParameterName { get; private set; }
    public int DeathParameterName { get; private set; } 

    public void InitializeAnim()
    {
        MoveParameterName = Animator.StringToHash(moveParameterName);
        AttackParameterName = Animator.StringToHash(attackParameterName);
        GuardParameterName = Animator.StringToHash(guardParamterName);
        DeathParameterName = Animator.StringToHash(deathParameterName);
    }
}
