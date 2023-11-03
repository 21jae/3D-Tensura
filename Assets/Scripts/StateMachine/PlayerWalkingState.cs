using UnityEngine;

public class PlayerWalkingState : PlayerGroundedState
{
    public PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region IState �޼���
    public override void Enter()
    {
        base.Enter();

        stateMachine.ReusableData.MovementSpeedModifier = movementData.WalkData.SpeedModifier;
    }
    #endregion

        
}
