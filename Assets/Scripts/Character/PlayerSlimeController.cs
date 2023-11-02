using System;
using System.Collections;
using UnityEngine;

public class PlayerSlimeController : MonoBehaviour
{
    [HideInInspector] public CharacterStatManager playerStatManager;
    [HideInInspector] public Animator animator;
    [SerializeField] private GameObject waterAttack;
    private Joystick controller;
    private MoveObject moveObject;
    private Rigidbody rigidbody;

    [SerializeField] private GameObject playerCanvas;



    private void Awake()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        playerStatManager = GetComponent<CharacterStatManager>();
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
        GameObject waterAttackInstance = ObjectPool.instance.GetPooledObject("SlimePoision");
        if (waterAttackInstance != null)
        {
            waterAttackInstance.transform.position = transform.position + transform.forward * 0.5f + Vector3.up * 0.5f;
            waterAttackInstance.transform.rotation = transform.rotation;
            waterAttackInstance.SetActive(true);

            StartCoroutine(ReturnToPoolAfterDelay(waterAttackInstance, 2.5f));
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(GameObject objectToReturn, float delay)
    {
        yield return new WaitForSeconds(delay);
        ObjectPool.instance.ReturnObjectToPool("SlimePoision", objectToReturn);

    }
}
