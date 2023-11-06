using System;
using UnityEngine;

public class PlayerAirborneState : PlayerMovementState
{
    public PlayerAirborneState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        CheckForGround();
    }

    private void CheckForGround()
    {
        Vector3 origin = stateMachine.Player.transform.position + (Vector3.up * stateMachine.Player.Collider.bounds.extents.y);
        float distance = stateMachine.Player.Collider.bounds.extents.y + 0.1f;

        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, distance))
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }
    }
}
