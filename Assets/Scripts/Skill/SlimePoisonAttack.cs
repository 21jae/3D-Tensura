using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimePoisonAttack : MonoBehaviour
{
    private PlayerSlimeController slimeController;

    private void Awake()
    {
        slimeController = GetComponent<PlayerSlimeController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        float damageToDeal = slimeController.playerStatManager.currentAttackPower;
        if (damageable != null)
        {
            damageable.TakeDamage(damageToDeal);
        }
    }
}
