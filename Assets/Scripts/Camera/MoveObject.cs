using System;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public Transform camPivot;
    private Joystick controller;
    private CharacterController characterController;
    private Rigidbody rb;

    public float speed = 3f;

    private void Awake()
    {
        rb = player1.GetComponent<Rigidbody>();
        characterController = player2.GetComponent<CharacterController>();
        controller = GetComponent<Joystick>();
    }

    private void FixedUpdate()
    {
        if (!UIInventory.INVENTORY_ACTIVATED)
        {
            if (player1)
            {
                Vector3 slimeDirection = Vector3.forward * controller.Vertical + Vector3.right * controller.Horizontal;

                if (slimeDirection != Vector3.zero)
                {
                    slimeDirection.Normalize();
                    Quaternion camPivotRotation = Quaternion.Euler(0f, camPivot.eulerAngles.y, 0f);
                    Vector3 alignedDirection = camPivotRotation * slimeDirection;
                    player1.rotation = Quaternion.LookRotation(alignedDirection);
                    rb.MovePosition(rb.position + alignedDirection * speed * Time.fixedDeltaTime);
                }
            }

            if (player2)
            {
                Vector3 humanDirection = Vector3.forward * controller.Vertical;
                humanDirection += Vector3.right * controller.Horizontal;

                if (humanDirection == Vector3.zero) return;

                Vector3 dirAngle = Quaternion.LookRotation(humanDirection).eulerAngles;
                Vector3 camPivotAngle = camPivot.eulerAngles;

                Vector3 moveAngle = Vector3.up * (dirAngle.y + camPivotAngle.y);

                player2.rotation = Quaternion.Euler(moveAngle);
                characterController.Move(player2.forward * speed * Time.fixedDeltaTime);
            }
        }
    }
}
