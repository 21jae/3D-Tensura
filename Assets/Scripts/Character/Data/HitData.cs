using UnityEngine;

[System.Serializable]
public class HitData
{
    [field: SerializeField][field: Range(0f, 2f)] public float invincibilityDuration { get; private set; } = 1f;
    [field: SerializeField][field: Range(0f, 1f)] public float blinkDuration { get; private set; } = 0.1f;
    [field: SerializeField][field: Range(1f, 10f)] public float knockbackStrength { get; private set; } = 5f;
    [field: SerializeField][field: Range(0f, 1f)] public float knockbackDuration { get; private set; } = 0.1f;
    [field: SerializeField][field: Range(0f, 3f)] public float damageInterval { get; private set; } = 1.5f;
    [field: SerializeField] public bool isInvincible { get; private set; }
    [field: SerializeField] public bool isRecoveringForBigDamage { get; private set; }

    public void SetIsInvincible(bool value)
    {
        isInvincible = value;
    }
    public void SetIsRecoveringForBigDamage(bool value)
    {
        isRecoveringForBigDamage = value;
    }
}
