using System;
using UnityEngine;

public class PlayerJumpingState : PlayerAirborneState
{
    //private float timeSinceLastJump = 0f;   //점프후 장애물에 부딪히면 Falling이 되지 않길 원하므로 약간의 버퍼링 추가

    public PlayerJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.ReusableData.MovementSpeedModifier = 0f;
        //timeSinceLastJump = 0f;
        Jump();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        //timeSinceLastJump > airborneData.JumpData.JumpBufferTime && 
        if (GetPlayerVerticalVelocity().y > 0f)
        {
            return;
        }

        stateMachine.ChangeState(stateMachine.FallingState);
        //timeSinceLastJump += Time.fixedDeltaTime;
    }

    private void Jump()
    {
        Vector3 jumpVector = new Vector3(0f, airborneData.JumpData.JumpForce, 0f);
        stateMachine.Player.Rigidbody.AddForce(jumpVector, ForceMode.Impulse);
    }
}
