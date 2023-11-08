using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePoisonAttack : MonoBehaviour
{
    private PlayerSlimeController slimeController;

    private void Awake()
    {
        slimeController = FindObjectOfType<PlayerSlimeController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        float damageToDeal = slimeController.playerStatManager.currentData.currentAttackPower;
        if (damageable != null)
        {
            damageable.TakeDamage(damageToDeal);
        }
    }
}
