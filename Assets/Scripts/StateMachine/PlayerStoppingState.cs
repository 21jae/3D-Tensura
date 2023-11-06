using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStoppingState : PlayerGroundedState
{
    public PlayerStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.ReusableData.MovementSpeedModifier = 0f;   //Stop �������϶� �ӵ��� 0�̶� �����ϼ����� (Player.Move())
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!IsMovingHorizontally())
        {
            return;
        }

        DecelerateHorizontally();
    }

    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();
        stateMachine.Player.Input.PlayerActions.Movement.started += OnMovementStarted;
    }


    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();
        stateMachine.Player.Input.PlayerActions.Movement.started -= OnMovementStarted;
    }

    public override void OnAnimationTransitionEvent()
    {
        stateMachine.ChangeState(stateMachine.IdlingState);
    }

    private void OnMovementStarted(InputAction.CallbackContext context)
    {
        OnMove();
    }
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
    }
}
