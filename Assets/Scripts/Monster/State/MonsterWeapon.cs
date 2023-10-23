using UnityEngine;

public class MonsterWeapon : MonoBehaviour
{
    [SerializeField] private CharacterStatManager enemyStatManager;
    [SerializeField] private LayerMask playerLayer;
    private float damageRadius = 5f;

    public void DealDamageToPlayersInRadius()
    {
        Collider[] hitPlayer = Physics.OverlapSphere(transform.position, damageRadius, playerLayer);

        float damageToDeal = enemyStatManager.currentAttackPower;

        foreach (var player in hitPlayer)
        {
            IDamageable damageablePlayer = player.GetComponent<IDamageable>();

            if (damageablePlayer != null)
            {
                damageablePlayer.TakeDamage(damageToDeal);
            }
        }
    }

}