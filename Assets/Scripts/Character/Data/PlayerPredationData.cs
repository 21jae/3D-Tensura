using TMPro;
using UnityEngine;

[System.Serializable]
public class PlayerPredationData
{
    [field: Header("흡수 데이터")]
    [field: SerializeField] public SOSkill predationSkillData { get; private set; }
    [field: SerializeField] public TMP_Text absorbedObjectText { get; private set; }

    [field: Header("흡수 프리팹")]
    [field: SerializeField] public GameObject predationPrefab {get; private set;}
    [field: SerializeField] public GameObject predationPosPrefab {get; private set;}
    [field: SerializeField] public GameObject predationPosition { get; private set; }

    [field: Header("흡수 변수")]
    [field: SerializeField] [field:Range(5f, 25f)] public float predationRaidus { get; private set; } = 20f;
    [field: SerializeField] [field:Range(5f, 25f)] public float predationForce { get; private set; } = 15f;

    [field: Header("흡수 설정")]
    [field: SerializeField] public float PREDATION_ANGLE { get; private set; } = 60f;      //흡수 각도
    [field: SerializeField] public float PREDATION_DURATION { get; private set; } = 3f;    //지속 시간
    [field: SerializeField] public float THRESHOLD { get; private set; } = 3f;             //범위
    [field: SerializeField] public bool isPredationActive { get; private set; } = false;

    [field: Header("데미지 딜레이")]
    [field: SerializeField] [field: Range(0f, 2f)] public float damageCooldown { get; private set; } = 0.5f;
    [field: SerializeField] public float lastDamageTIme { get; set; }

    public void SetActivePredation(bool predation)
    {
        isPredationActive = predation;
    }
}
