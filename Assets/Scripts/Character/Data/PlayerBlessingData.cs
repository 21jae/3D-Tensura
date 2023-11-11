using UnityEngine;

[System.Serializable]
public class PlayerBlessingData
{
    [field: Header("가호 프리팹")]
    [field: SerializeField] public GameObject blessingHuman { get; private set;}
    [field: SerializeField] public GameObject blessingEffect {get; private set; }
    [field: SerializeField] public GameObject buffEffect {get; private set; }

    [field: Header("가호 변수")]
    [field: SerializeField][field: Range(0f, 3f)] public float offsetDistance { get; private set; } = 2f;
    [field: SerializeField] public float destoryPrefabDuration { get; private set; } = 4f;
    [field: SerializeField][field: Range(0f, 30f)] public float buffDuration { get; private set; } = 15f;


    [field: Header("가호 버프 변수")]
    [field: SerializeField][field: Range(0f, 1f)] public float healthBuffPercentage { get; private set; } = 0.05f;
    [field: SerializeField][field: Range(0f, 1f)] public float attackPowerBuffPercentage { get; private set; } = 0.2f;
    [field: SerializeField][field: Range(0f, 1f)] public float defenseBuffPercentage { get; private set; } = 0.2f;
}
