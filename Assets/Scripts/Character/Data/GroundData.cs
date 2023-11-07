using UnityEngine;

[System.Serializable]
public class GroundData
{
    [field: SerializeField] public AnimationData AnimationData { get; private set; }
    [field: SerializeField] public AttackData AttackData { get; private set; }
    [field: SerializeField] public HitData HitData { get; private set; }
    [field: SerializeField] public LayerData LayerData { get; private set; }
    [field: SerializeField] public CheckData CheckData { get; private set; }
    [field: SerializeField] public WallData WallData { get; private set; }

}
