using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private CharacterStatManager playerStatManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();

            if (damageable != null) //IDamageable 인터페이스를 구현하고있는지 확인
            {
                float damageToDeal = playerStatManager.currentAttackPower;
                damageable.TakeDamage(damageToDeal);
            }
        }
    }
}
