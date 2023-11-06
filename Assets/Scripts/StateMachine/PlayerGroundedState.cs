using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerMovementState
{
    public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region IState 메서드
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        FloatCapsule();

    }

    #endregion

    #region 메인 메서드
    private void FloatCapsule()
    {
        Ray downRayFromPlayer = new Ray(stateMachine.Player.transform.position, Vector3.down);

        if (Physics.Raycast(downRayFromPlayer, out RaycastHit hit, movementData.FloatRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
        {
            float groundAngle = Vector3.Angle(hit.normal, Vector3.up);
            float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

            if (groundAngle > movementData.SlopeLimit)
            {
                return; // 높은 각도에서는 못 올라감
            }

            if (groundAngle <= 20f && groundAngle <= 45f)
            {
                stateMachine.Player.Rigidbody.AddForce(Vector3.down * stateMachine.Player.Rigidbody.mass * 1.5f ,ForceMode.Acceleration);
            }

            if (slopeSpeedModifier > 0)
            {
                // 경사면에서 중력을 조정하여 플레이어를 지면에 단단히 고정시킵니다.
                if (groundAngle > 55)
                {
                    // 55도를 넘는 경사면에서는 중력을 추가하여 플레이어가 더 단단히 지면에 붙게 합니다.
                    stateMachine.Player.Rigidbody.AddForce(Vector3.down * stateMachine.Player.Rigidbody.mass * (groundAngle / movementData.SlopeLimit), ForceMode.Acceleration);
                }
            }
            else
            {
                // 경사면의 각도가 너무 가파르면 중력을 더 많이 적용하여 미끄러지지 않게 합니다.
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

    #region 재사용 가능 메서드
    protected virtual void OnMove()
    {
        if (stateMachine.ReusableData.ShouldWalk)
        {
            stateMachine.ChangeState(stateMachine.WalkingState);

            return;
        }

        stateMachine.ChangeState(stateMachine.RunningState);
    }

    //콜백
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


    #region 입력 방식

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
