using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "RPG/Characters/Character")]
public class PlayerSO : ScriptableObject
{
    [field: SerializeField] public GroundData GroundData { get; private set; }
    [field: SerializeField] public AirData AirData { get; private set; }
}
