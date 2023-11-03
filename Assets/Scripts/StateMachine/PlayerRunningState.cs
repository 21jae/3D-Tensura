using UnityEngine.InputSystem;

public class PlayerRunningState : PlayerGroundedState
{
    public PlayerRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region IState 메서드
    public override void Enter()
    {
        base.Enter();

        stateMachine.ReusableData.MovementSpeedModifier = movementData.RunData.SpeedModifier;
    }
    #endregion

    #region 입력 방식

    protected override void OnWalkToggleStated(InputAction.CallbackContext context) //만약 또 눌렀다면
    {
        base.OnWalkToggleStated(context);

        stateMachine.ChangeState(stateMachine.WalkingState);
    }
    #endregion
}
