using UnityEngine;

[System.Serializable]
public class PlayerSprintData
{
    [field: SerializeField][field: Range(1f, 5f)] public float SpeedModifier { get; private set; } = 3f;

}
