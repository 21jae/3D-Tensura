using System;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public Transform player;
    public Transform camPivot;
    private Joystick controller;
    //private CharacterController characterController;
    private Rigidbody rb;

    public float speed = 3f;

    private void Awake()
    {
        rb = player.GetComponent<Rigidbody>();
        controller = GetComponent<Joystick>();
        //characterController = player.GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        if (!UIInventory.INVENTORY_ACTIVATED)
        {
            //Vector3 direction = Vector3.forward * controller.Vertical;
            //direction += Vector3.right * controller.Horizontal;

            //if (direction == Vector3.zero) return;

            //Vector3 dirAngle = Quaternion.LookRotation(direction).eulerAngles;
            //Vector3 camPivotAngle = camPivot.eulerAngles;

            //Vector3 moveAngle = Vector3.up * (dirAngle.y + camPivotAngle.y);

            //player.rotation = Quaternion.Euler(moveAngle);
            //characterController.Move(player.forward * speed * Time.fixedDeltaTime);

            Vector3 direction = Vector3.forward * controller.Vertical + Vector3.right * controller.Horizontal;

            if (direction != Vector3.zero)
            {
                direction.Normalize();
                Quaternion camPivotRotation = Quaternion.Euler(0f, camPivot.eulerAngles.y, 0f);
                Vector3 alignedDirection = camPivotRotation * direction;
                player.rotation = Quaternion.LookRotation(alignedDirection);
                rb.MovePosition(rb.position + alignedDirection * speed * Time.fixedDeltaTime);
            }
        }
    }
}
