using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkingState : PlayerGroundedState
{
    public PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region IState ¸Þ¼­µå
    public override void Enter()
    {
        base.Enter();

        stateMachine.ReusableData.MovementSpeedModifier = movementData.WalkData.SpeedModifier;
    }
    #endregion

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.LightStoppingState);
    }

    protected override void OnWalkToggleStated(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStated(context);

        stateMachine.ChangeState(stateMachine.RunningState);
    }
}
