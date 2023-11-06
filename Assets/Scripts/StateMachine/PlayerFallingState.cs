using System;
using UnityEngine;

public class PlayerFallingState : PlayerAirborneState
{
    public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.ReusableData.MovementSpeedModifier = 0f;

        LimitVerticalVelocity();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }


    private void LimitVerticalVelocity()
    {
        //낙하 속도 제한
    }

}
