using UnityEngine;

[System.Serializable]
public class PlayerMegidoData
{
    [field: SerializeField] [field: Range(0f, 90f)] public float skillDelay { get; private set; } = 1f;

    [field: Header("도약")]
    [field: SerializeField] [field: Range(10f, 20f)] public float jumpHeight { get; private set; } = 15f;
    [field: SerializeField] [field: Range(10f, 20f)] public float jumpSpeed { get; private set; } = 10f;

    [field: SerializeField] public GameObject wingMesh { get; private set; }
    [field: SerializeField] public GameObject maskMesh { get; private set; }
    [field: SerializeField] public GameObject swordMesh { get; private set; }

    [field: Header("스킬 데이터")]
    [field: SerializeField] public SOSkill specialSkillData { get; private set; }
    [field: SerializeField] public GameObject megidoCircle { get; private set; }
    [field: SerializeField] public Transform megidoPosPrefab { get; private set; }
    [field: SerializeField] public Transform megidoTargetPrefab { get; private set; }
    [field: SerializeField] public GameObject megidoRayPrefab { get; private set; }
    [field: SerializeField] public GameObject megidoHitPrefab { get; private set; }
    [field: SerializeField] public GameObject megidoExplosion { get; private set; }

    // 스킬의 시각적 효과와 관련된 설정 값
    [field: SerializeField][field: Range(0.01f, 1f)] public float rayWidth { get; private set; } = 0.1f;
    [field: SerializeField][field: Range(10f, 100f)] public float explosionRadius { get; private set; } = 50f;
}
