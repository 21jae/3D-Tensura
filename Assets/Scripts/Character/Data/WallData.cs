using UnityEngine;

[System.Serializable]
public class WallData
{
    [field: SerializeField][field: Range(0f, 3f)] public float rayDistance { get; private set; } = 1.5f;
    [field: SerializeField] public int wallLayerMask = 1 << 6;
}
