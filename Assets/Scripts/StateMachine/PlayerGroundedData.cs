using UnityEngine;

[System.Serializable]
public class PlayerGroundedData
{
    [field: SerializeField] [field: Range(0f, 10f)] public float BaseSpeed { get; private set; } = 5f;
    [field: SerializeField] public PlayerRotationData BaseRotationData { get; private set; }
    [field: SerializeField] public PlayerWalkData WalkData { get; private set; }
    [field: SerializeField] public PlayerRunData RunData { get; private set; }
}
