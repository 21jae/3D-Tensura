using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementState : IState
{
    protected PlayerMovementStateMachine stateMachine;
    protected PlayerGroundedData movementData;

    public PlayerMovementState(PlayerMovementStateMachine playerMovementStateMachine)   //playerStateMachine.Player. ���̱����Ͽ� ����
    {
        stateMachine = playerMovementStateMachine;

        movementData = stateMachine.Player.Data.GroundedData;
    }

    #region IState �޼���
    public virtual void Enter()
    {
        Debug.Log("State : " + GetType().Name);

        AddInputActionsCallbacks();
    }


    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();    
    }

    public virtual void PhysicsUpdate()
    {
        Move();
    }

    public virtual void Update()
    {
        
    }
    #endregion

    #region ���� �޼���
    private void ReadMovementInput()
    {
        stateMachine.ReusableData.MovementInput = stateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();  //����ȭ �۾��� �ʿ���� PlayerInput <Vector2>
    }

    private void Move()
    {
        if (stateMachine.ReusableData.MovementInput == Vector2.zero || stateMachine.ReusableData.MovementSpeedModifier == 0f)
        {
            return;
        }

        Vector3 movementDirection = GetMovementInputDirection();

        float movementSpeed = GetMovementSpeed();

        Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

        stateMachine.Player.Rigidbody.AddForce(movementDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
    }

    #endregion

    #region ���� ������ �޼���
    protected Vector3 GetMovementInputDirection()
    {
        return new Vector3(stateMachine.ReusableData.MovementInput.x, 0f, stateMachine.ReusableData.MovementInput.y);
    }

    protected float GetMovementSpeed()
    {
        return movementData.BaseSpeed * stateMachine.ReusableData.MovementSpeedModifier;
    }

    protected Vector3 GetPlayerHorizontalVelocity() //AddForce�� �̹� �����ϴ� ���� �ӵ��� �߰��ϹǷ� ���� �ӵ��� �����ؾ߸� �Ѵ�.
    {
        Vector3 playerHorizontalVelocity = stateMachine.Player.Rigidbody.velocity;

        playerHorizontalVelocity.y = 0f;

        return playerHorizontalVelocity;
    }

    protected void ResetVelocity()
    {
        stateMachine.Player.Rigidbody.velocity = Vector3.zero;
    }


    protected virtual void AddInputActionsCallbacks()
    {
        stateMachine.Player.Input.PlayerActions.WalkToggle.started += OnWalkToggleStated;
    }


    protected virtual void RemoveInputActionsCallbacks()
    {
        stateMachine.Player.Input.PlayerActions.WalkToggle.started -= OnWalkToggleStated;
    }
    #endregion

    #region �Է� �޼���
    protected virtual void OnWalkToggleStated(InputAction.CallbackContext context)
    {
        stateMachine.ReusableData.ShouldWalk = !stateMachine.ReusableData.ShouldWalk;   //Ű�� ���������� bool�� ����
    }
    #endregion
}
