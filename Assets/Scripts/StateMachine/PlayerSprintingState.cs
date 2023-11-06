using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintingState : PlayerGroundedState
{
    private PlayerSprintData sprintData;
    private bool isSprintKeyPressed; //키가 충분히 오랫동안 유지되면

    public PlayerSprintingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        sprintData = movementData.SprintData;
    }

    public override void Enter()
    {
        base.Enter();
        isSprintKeyPressed = true;
        stateMachine.ReusableData.MovementSpeedModifier = sprintData.SpeedModifier;
    }

    public override void Update()
    {
        base.Update();
        if (!isSprintKeyPressed)
        {
            StopSprinting();
        }
    }

    public override void Exit()
    {
        base.Exit();

    }

    private void StopSprinting()
    {
        if (stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
            return;
        }
        else
        {
            stateMachine.ChangeState(stateMachine.RunningState);
        }
    }

    //콜백
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.HardStoppingState);
    }
    protected override void AddInputActionsCallbacks()
    {
        stateMachine.Player.Input.PlayerActions.Sprint.canceled += OnSprintCanceld;
    }

    protected override void RemoveInputActionsCallbacks()
    {
        stateMachine.Player.Input.PlayerActions.Sprint.canceled -= OnSprintCanceld;
    }
    private void OnSprintCanceld(InputAction.CallbackContext context)
    {
        isSprintKeyPressed = false;
    }

}
