using UnityEngine;

[System.Serializable]
public class PlayerBlessingData
{
    [field: Header("가호 변수")]
    [field: SerializeField][field: Range(0f, 3f)] public float offsetDistance { get; private set; } = 2f;
    [field: SerializeField] public float destoryPrefabDuration { get; private set; } = 4f;
    [field: SerializeField][field: Range(0f, 30f)] public float buffDuration { get; private set; } = 15f;


    [field: Header("버프 변수")]
    [field: SerializeField][field: Range(0f, 1f)] public float healthBuffPercentage { get; private set; } = 0.3f;
    [field: SerializeField][field: Range(0f, 1f)] public float attackPowerBuffPercentage { get; private set; } = 0.2f;
    [field: SerializeField][field: Range(0f, 1f)] public float defenseBuffPercentage { get; private set; } = 0.2f;
}
