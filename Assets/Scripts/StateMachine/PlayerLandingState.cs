
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLandingState : PlayerGroundedState
{
    public PlayerLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        //������ Ű �ȴ����� Idle
    }
}
