
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLandingState : PlayerGroundedState
{
    public PlayerLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        //랜딩후 키 안누르면 Idle
    }
}
