using UnityEngine;

public class PlayerIdlingState : PlayerGroundedState
{
    public PlayerIdlingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region IState �޼���
    public override void Enter()
    {
        base.Enter();

        stateMachine.ReusableData.MovementSpeedModifier = 0f;

        ResetVelocity();    //speedModifier�� 0�� ����� �̲������� �����Ƿ�, �ﰢ �ӵ� ����
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.ReusableData.MovementInput == Vector2.zero) 
        {
            return;
        }

        OnMove();
    }
    #endregion

}
