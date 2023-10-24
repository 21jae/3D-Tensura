using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float amount, bool isPredation = false);
}
