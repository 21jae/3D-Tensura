using UnityEngine;

[System.Serializable]
public class PlayerMegidoData
{
    [field: Header("오의 데이터 ")]
    [field: SerializeField] public SOSkill specialSkillData { get; private set; }

    [field: Header("오의 프리팹")]
    [field: SerializeField] public GameObject megidoCircle { get; private set; }
    [field: SerializeField] public GameObject megidoHit { get; private set; }
    [field: SerializeField] public GameObject megidoExplosion { get; private set; }
    [field: SerializeField] public GameObject skillCutScenes { get; private set; }

    [field: Header("도약 설정")]
    [field: SerializeField] public GameObject wingMesh { get; private set; }
    [field: SerializeField] public GameObject maskMesh { get; private set; }
    [field: SerializeField] public GameObject swordMesh { get; private set; }
    [field: SerializeField] [field: Range(10f, 20f)] public float jumpHeight { get; private set; } = 15f;
    [field: SerializeField] [field: Range(10f, 20f)] public float jumpSpeed { get; private set; } = 10f;

    [field: Header("오의 변수")]
    [field: SerializeField] [field: Range(0f, 90f)] public float skillDelay { get; private set; } = 1f;
    [field: SerializeField] [field: Range(0.01f, 1f)] public float rayWidth { get; private set; } = 0.1f;
    [field: SerializeField] [field: Range(10f, 100f)] public float explosionRadius { get; private set; } = 50f;
}
