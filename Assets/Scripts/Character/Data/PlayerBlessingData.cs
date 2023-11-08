using UnityEngine;

[System.Serializable]
public class PlayerBlessingData
{
    [field: Header("��ȣ ����")]
    [field: SerializeField][field: Range(0f, 3f)] public float offsetDistance { get; private set; } = 2f;
    [field: SerializeField] public float destoryPrefabDuration { get; private set; } = 4f;
    [field: SerializeField][field: Range(0f, 30f)] public float buffDuration { get; private set; } = 15f;


    [field: Header("���� ����")]
    [field: SerializeField][field: Range(0f, 1f)] public float healthBuffPercentage { get; private set; } = 0.3f;
    [field: SerializeField][field: Range(0f, 1f)] public float attackPowerBuffPercentage { get; private set; } = 0.2f;
    [field: SerializeField][field: Range(0f, 1f)] public float defenseBuffPercentage { get; private set; } = 0.2f;
}
