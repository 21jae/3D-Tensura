using UnityEngine;

[System.Serializable]
public class PlayerStopData
{
    [field: SerializeField][field: Range(0f, 15f)] public float LightDecelerationFroce { get; private set; } = 5f;
    [field: SerializeField][field: Range(0f, 15f)] public float MediumDecelerationFroce { get; private set; } = 6.5f;
    [field: SerializeField][field: Range(0f, 15f)] public float HardDecelerationFroce { get; private set; } = 5f;

}
