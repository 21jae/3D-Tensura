using UnityEngine;

public class MonsterWeapon : MonoBehaviour
{
    private Enemy enemy;
    
    [SerializeField] private PlayerStatManager enemyStatManager;
    [SerializeField] private LayerMask playerLayer;
    private float damageRadius = 5f;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }


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
