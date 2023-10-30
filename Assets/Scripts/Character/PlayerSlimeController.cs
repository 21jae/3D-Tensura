using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlimeController : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    private Joystick controller;
    private MoveObject moveObject;
    private Rigidbody rbody;
   // private CharacterController characterController;

    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        rbody = GetComponent<Rigidbody>();
       // characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        controller = FindObjectOfType<Joystick>();
        moveObject = GetComponent<MoveObject>();
    }

    private void Update()
    {
        MovementUpdate();
    }

    private void MovementUpdate()
    {
        float moveSpeed = controller.Direction.magnitude;
        animator.SetFloat("MoveSpeed", moveSpeed);
    }
}
