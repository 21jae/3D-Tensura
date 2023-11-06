using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerMovementState
{
    public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region IState �޼���
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        FloatCapsule();

    }

    #endregion

    #region ���� �޼���
    private void FloatCapsule()
    {
        Ray downRayFromPlayer = new Ray(stateMachine.Player.transform.position, Vector3.down);

        if (Physics.Raycast(downRayFromPlayer, out RaycastHit hit, movementData.FloatRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
        {
            float groundAngle = Vector3.Angle(hit.normal, Vector3.up);
            float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

            if (groundAngle > movementData.SlopeLimit)
            {
                return; // ���� ���������� �� �ö�
            }

            if (groundAngle <= 20f && groundAngle <= 45f)
            {
                stateMachine.Player.Rigidbody.AddForce(Vector3.down * stateMachine.Player.Rigidbody.mass * 1.5f ,ForceMode.Acceleration);
            }

            if (slopeSpeedModifier > 0)
            {
                // ���鿡�� �߷��� �����Ͽ� �÷��̾ ���鿡 �ܴ��� ������ŵ�ϴ�.
                if (groundAngle > 55)
                {
                    // 55���� �Ѵ� ���鿡���� �߷��� �߰��Ͽ� �÷��̾ �� �ܴ��� ���鿡 �ٰ� �մϴ�.
                    stateMachine.Player.Rigidbody.AddForce(Vector3.down * stateMachine.Player.Rigidbody.mass * (groundAngle / movementData.SlopeLimit), ForceMode.Acceleration);
                }
            }
            else
            {
                // ������ ������ �ʹ� ���ĸ��� �߷��� �� ���� �����Ͽ� �̲������� �ʰ� �մϴ�.
                stateMachine.Player.Rigidbody.AddForce(Vector3.down * stateMachine.Player.Rigidbody.mass * 2f, ForceMode.Acceleration);
            }
        }
    }

    private float SetSlopeSpeedModifierOnAngle(float angle)
    {
        float slopeSpeedModifier = movementData.SlopeSpeedAngles.Evaluate(angle);
        stateMachine.ReusableData.MovementOnSlopeSpeedModifier = slopeSpeedModifier;
        return slopeSpeedModifier;
    }



    #endregion

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
        stateMachine.Player.Input.PlayerActions.Sprint.started += OnSprintStarted;
        stateMachine.Player.Input.PlayerActions.Jump.started += OnJumpStarted;
    }

    protected override void RemoveInputActionsCallbacks()
    {
        base.RemoveInputActionsCallbacks();

        stateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;
        stateMachine.Player.Input.PlayerActions.Sprint.started -= OnSprintStarted;
        stateMachine.Player.Input.PlayerActions.Jump.started -= OnJumpStarted;
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


    protected virtual void OnSprintStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.SprintingState);
    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.JumpingState);
    }
    #endregion
}
