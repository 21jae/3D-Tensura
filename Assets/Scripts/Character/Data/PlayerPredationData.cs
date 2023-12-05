using TMPro;
using UnityEngine;

[System.Serializable]
public class PlayerPredationData
{
    [field: Header("��� ������")]
    [field: SerializeField] public SOSkill predationSkillData { get; private set; }
    [field: SerializeField] public TMP_Text absorbedObjectText { get; private set; }

    [field: Header("��� ������")]
    [field: SerializeField] public GameObject predationPrefab {get; private set;}
    [field: SerializeField] public GameObject predationPosPrefab {get; private set;}
    [field: SerializeField] public GameObject predationPosition { get; private set; }

    [field: Header("��� ����")]
    [field: SerializeField] [field:Range(5f, 25f)] public float predationRaidus { get; private set; } = 20f;
    [field: SerializeField] [field:Range(5f, 25f)] public float predationForce { get; private set; } = 15f;

    [field: Header("��� ����")]
    [field: SerializeField] public float PREDATION_ANGLE { get; private set; } = 60f;      //��� ����
    [field: SerializeField] public float PREDATION_DURATION { get; private set; } = 3f;    //���� �ð�
    [field: SerializeField] public float THRESHOLD { get; private set; } = 3f;             //����
    [field: SerializeField] public bool isPredationActive { get; private set; } = false;

    [field: Header("������ ������")]
    [field: SerializeField] [field: Range(0f, 2f)] public float damageCooldown { get; private set; } = 0.5f;
    [field: SerializeField] public float lastDamageTIme { get; set; }

    public void SetActivePredation(bool predation)
    {
        isPredationActive = predation;
    }
}
