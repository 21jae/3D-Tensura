using System;
using UnityEngine;

public class PlayerJumpingState : PlayerAirborneState
{
    private PlayerJumpData jumpData;
    public PlayerJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        jumpData = airborneData.JumpData;
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.ReusableData.MovementSpeedModifier = 0f;
        Jump();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void Jump()
    {
        Vector3 jumpVector = new Vector3(0f, jumpData.JumpForce, 0f);
        stateMachine.Player.Rigidbody.AddForce(jumpVector, ForceMode.Impulse);
    }
}
