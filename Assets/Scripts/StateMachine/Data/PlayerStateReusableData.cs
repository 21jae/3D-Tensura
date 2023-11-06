using UnityEngine;

public class PlayerStateReusableData 
{
    public Vector2 MovementInput { get; set; }
    public float MovementSpeedModifier { get; set; } = 1f;
    public float MovementOnSlopeSpeedModifier { get; set; } = 1f;   //°æ»ç¸é
    public float MovementDecelerationForce { get; set; } = 1f;
    public bool ShouldWalk { get; set; }
    public Vector3 CurrentJumpForce { get; set; }
}
