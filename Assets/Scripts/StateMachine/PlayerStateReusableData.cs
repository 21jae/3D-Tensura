using UnityEngine;

public class PlayerStateReusableData 
{
    public Vector2 MovementInput { get; set; }
    public float MovementSpeedModifier { get; set; } = 1f;
    public float MovementOnSlopeSpeedModifier { get; set; } = 1f;   //°æ»ç¸é
    public bool ShouldWalk { get; set; }
}
