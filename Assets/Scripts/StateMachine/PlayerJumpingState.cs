using System;
using UnityEngine;

public class PlayerJumpingState : PlayerAirborneState
{
    //private float timeSinceLastJump = 0f;   //������ ��ֹ��� �ε����� Falling�� ���� �ʱ� ���ϹǷ� �ణ�� ���۸� �߰�

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
