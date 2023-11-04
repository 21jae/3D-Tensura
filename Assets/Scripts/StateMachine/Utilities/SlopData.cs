using UnityEngine;

[System.Serializable]
public class SlopData
{
    [field: SerializeField][field: Range(0f, 1f)] public float StepHeightPercentage { get; private set; }
}
