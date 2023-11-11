using UnityEngine;

[System.Serializable]
public class PlayerSkillData
{
    [field: Header("스킬 데이터")]
    [field: SerializeField] public PlayerBuffData BuffData { get; private set; }
    [field: SerializeField] public PlayerBlessingData BlessingData { get; private set; }
    [field: SerializeField] public PlayerDashData DashData { get; private set; }
    [field: SerializeField] public PlayerPredationData PredationData { get; private set; }
    [field: SerializeField] public PlayerMegidoData MegidoData { get; private set; }
}
