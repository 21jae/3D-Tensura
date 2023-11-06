using UnityEngine;

[System.Serializable]
public class PlayerGroundedData
{
    [field: SerializeField] [field: Range(0f, 10f)] public float BaseSpeed { get; private set; } = 5f;
    [field: SerializeField] [field: Range(0f, 5f)] public float FloatRayDistance { get; private set; } = 1.5f;
    [field: SerializeField][field: Range(0f, 5f)] public float SlopeGravityMultiplier { get; private set; } = 2f;
    [field: SerializeField] public float SlopeLimit { get; private set; } = 65f;
    [field: SerializeField] public AnimationCurve SlopeSpeedAngles { get; private set; }
    [field: SerializeField] public PlayerRotationData BaseRotationData { get; private set; }
    [field: SerializeField] public PlayerWalkData WalkData { get; private set; }
    [field: SerializeField] public PlayerRunData RunData { get; private set; }
    [field: SerializeField] public PlayerSprintData SprintData { get; private set; }
    [field: SerializeField] public PlayerStopData StopData { get; private set; }
}
