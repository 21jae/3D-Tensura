using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerMovementState
{
    public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }




    #region ���� ���� �޼���


    protected virtual void OnMove()
    {
        if (stateMachine.ReusableData.ShouldWalk)
        {
            stateMachine.ChangeState(stateMachine.WalkingState);

            return;
        }

        stateMachine.ChangeState(stateMachine.RunningState);
    }


    //�ݹ�
    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();

        stateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;
    }

    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();

        stateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;
    }
    #endregion


    #region �Է� ���

    protected override void OnWalkToggleStated(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStated(context);

        stateMachine.ChangeState(stateMachine.RunningState);
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.IdlingState);
    }

    #endregion
}
