using UnityEngine;

[System.Serializable]
public class PlayerJumpData
{
    [field: SerializeField][field: Range(0f, 5f)] public float GroundCheckDistance { get; private set; } = 2f;
    [field: SerializeField][field: Range(0f, 15f)] public float JumpForce { get; private set; } = 5f;
    [field: SerializeField][field: Range(0f, 1f)] public float JumpBufferTime { get; private set; } = 0.2f;
    [field: SerializeField] public AnimationCurve JumpForceModifierOnSlopeUpwards { get; private set; }
    [field: SerializeField] public AnimationCurve JumpForceModifierOnSlopeDownwards { get; private set; }
}
