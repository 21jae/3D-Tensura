using UnityEngine.InputSystem;

public class PlayerRunningState : PlayerGroundedState
{
    public PlayerRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region IState �޼���
    public override void Enter()
    {
        base.Enter();

        stateMachine.ReusableData.MovementSpeedModifier = movementData.RunData.SpeedModifier;
    }
    #endregion

    #region �Է� ���

    protected override void OnWalkToggleStated(InputAction.CallbackContext context) //���� �� �����ٸ�
    {
        base.OnWalkToggleStated(context);

        stateMachine.ChangeState(stateMachine.WalkingState);
    }
    #endregion
}
