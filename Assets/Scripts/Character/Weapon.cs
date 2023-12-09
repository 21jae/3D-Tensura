using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private CharacterStatManager playerStatManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();

            if (damageable != null)
            {
                float damageToDeal = CharacterStatManager.instance.currentData.currentAttackPower;
                damageable.TakeDamage(damageToDeal);
            }
        }
    }
}
