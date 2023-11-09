using System.Collections;
using UnityEngine;

public class SlimePoisonSkill : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        float damageToDeal = CharacterStatManager.instance.currentData.currentAttackPower;
        ;
        if (damageable != null)
        {
            damageable.TakeDamage(damageToDeal);
        }
    }
}
