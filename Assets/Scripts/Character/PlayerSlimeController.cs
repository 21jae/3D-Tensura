using System;
using System.Collections;
using UnityEngine;

public class PlayerSlimeController : MonoBehaviour
{
    [HideInInspector] public CharacterStatManager playerStatManager;
    [HideInInspector] public Animator animator;
    [SerializeField] private GameObject waterAttack;
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private Texture2D cursurIcon;
    private Joystick controller;

    private void Awake()
    {
        InitializeComponents();
    }

    private void Start()
    {
        SoundManager.Instance.PlayBackgroundMusic();
    }

    private void InitializeComponents()
    {
        playerStatManager = GetComponent<CharacterStatManager>();
        animator = GetComponentInChildren<Animator>();
        controller = FindObjectOfType<Joystick>();
        Cursor.SetCursor(cursurIcon, Vector2.zero, CursorMode.Auto);
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
        GameObject waterAttackInstance = ObjectPooling.instance.GetPooledObject("SlimePoision");
        if (waterAttackInstance != null)
        {
            SoundManager.Instance.SlimeShootingSound();
            waterAttackInstance.transform.position = transform.position + transform.forward * 0.5f + Vector3.up * 0.5f;
            waterAttackInstance.transform.rotation = transform.rotation;
            waterAttackInstance.SetActive(true);

            StartCoroutine(ReturnToPoolAfterDelay(waterAttackInstance, 2.5f));
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(GameObject objectToReturn, float delay)
    {
        yield return CoroutineHelper.WaitForSeconds(delay);
        ObjectPooling.instance.ReturnObjectToPool("SlimePoision", objectToReturn);
    }
}