using UnityEngine;

[System.Serializable]
public class PlayerFallData 
{
    [field: SerializeField][field: Range(0f, 3f)] public float minimumFallingDistance { get; private set; } = 1f;
    [field: SerializeField][field: Range(1f, 15f)] public float FallSpeedLimit { get; private set; } = 10f;
}
