using UnityEngine;

[System.Serializable]
public class PlayerPredationData
{
    [field: SerializeField] [field:Range(5f, 25f)] public float predationRaidus { get; private set; } = 20f;
    [field: SerializeField] [field:Range(5f, 25f)] public float predationForce { get; private set; } = 15f;

    [field: Header("흡수 설정")]
    [field: SerializeField] public GameObject predationPosition { get; private set; }
    [field: SerializeField] public float PREDATION_ANGLE { get; private set; } = 60f;      //흡수 각도
    [field: SerializeField] public float PREDATION_DURATION { get; private set; } = 3f;    //지속 시간
    [field: SerializeField] public float THRESHOLD { get; private set; } = 3f;             //범위
    
    [field: SerializeField] public bool isPredationActive { get; set; }
}
