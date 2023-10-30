using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAttackButton : MonoBehaviour
{
    private PlayerSlimeController _playerSlimeController;

    private void Awake()
    {
        _playerSlimeController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSlimeController>();
    }

    public void OnPointerDown()
    {
        _playerSlimeController.StartAttack();
    }
}
