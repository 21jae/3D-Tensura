using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private CharacterStatManager playerStatManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();

            if (damageable != null) //IDamageable �������̽��� �����ϰ��ִ��� Ȯ��
            {
                float damageToDeal = playerStatManager.currentAttackPower;
                damageable.TakeDamage(damageToDeal);
            }
        }
    }
}
