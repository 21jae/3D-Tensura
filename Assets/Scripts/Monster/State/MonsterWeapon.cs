using UnityEngine;

public class MonsterWeapon : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float damageRadius = 12f;

    public void DealDamageToPlayersInRadius()
    {
        Collider[] hitPlayer = Physics.OverlapSphere(transform.position, damageRadius, playerLayer);

        float damageToDeal = CharacterStatManager.instance.currentData.currentAttackPower;
        Debug.Log("데미지1");

        foreach (var player in hitPlayer)
        {
            IDamageable damageablePlayer = player.GetComponent<IDamageable>();

            if (damageablePlayer != null)
            {
                damageablePlayer.TakeDamage(damageToDeal);
                Debug.Log("데미지2");
            }

        }
    }
}