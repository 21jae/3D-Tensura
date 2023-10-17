using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private CharacterStats playerStats;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();

            if (damageable != null) //IDamageable �������̽��� �����ϰ��ִ��� Ȯ��
            {
                float damageToDeal = playerStats.attackPower;
                damageable.TakeDamage(damageToDeal);
            }
        }
    }
}
    