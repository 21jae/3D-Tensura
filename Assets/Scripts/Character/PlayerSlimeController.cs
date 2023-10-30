using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlimeController : MonoBehaviour
{
    [SerializeField] private GameObject waterAttack;

    [HideInInspector] public Animator animator;
    private Joystick controller;
    private MoveObject moveObject;
    private Rigidbody rigidbody;

    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        rigidbody = GetComponent<Rigidbody>();
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

    public void StartAttack()
    {
        //UIAttackButton에서 버튼 누를시 호출
        //animator.SetTrigger("Attack");
        StartCoroutine(PerformAttack());
    }

    private IEnumerator PerformAttack()
    {
        float initialHeight = transform.position.y;
        float jumpHeight = 4f;
        rigidbody.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2 * Physics.gravity.y), ForceMode.VelocityChange);
        yield return new WaitUntil(() => transform.position.y >= initialHeight + 2);
        
        GameObject waterAttackInstance = Instantiate(waterAttack, transform.position + transform.forward * 1.5f, Quaternion.identity);
        Rigidbody waterAttackRigidbody = waterAttackInstance.GetComponent<Rigidbody>();

        if (waterAttackRigidbody != null)
        {
            waterAttackRigidbody.AddForce(transform.forward * 80f, ForceMode.Impulse);
        }
    }
}
