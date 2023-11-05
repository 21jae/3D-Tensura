using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerMovementState
{
    private SlopeData slopeData;
    public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        slopeData = stateMachine.Player.ColliderUtility.SlopeData;
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
        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;
        Ray downRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

        if (Physics.Raycast(downRayFromCapsuleCenter, out RaycastHit hit, slopeData.FloatRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
        {
            float groundAngle = Vector3.Angle(hit.normal, downRayFromCapsuleCenter.direction);
            float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

            if (slopeSpeedModifier == 0f)
            {
                return; //��� �ʹ� ������ ���ö�
            }

            float distanceToFloatingPoint = stateMachine.Player.ColliderUtility.CapsuleColliderData.ColliderCenterInLocalSpace.y * stateMachine.Player.transform.localScale.y -hit.distance;
            
            if (distanceToFloatingPoint == 0f)
            {
                return;
            }

            float amountToLift = distanceToFloatingPoint * slopeData.StepReachForce - GetPlayerVerticalVelocity().y;
            Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

            stateMachine.Player.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
        }
    }

    private float SetSlopeSpeedModifierOnAngle(float angle)
    {
        float slopSpeedModifier = movementData.SlopeSpeedAngles.Evaluate(angle);
        stateMachine.ReusableData.MovementOnSlopeSpeedModifier = slopSpeedModifier;
        return slopSpeedModifier;
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
