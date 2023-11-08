using UnityEngine;

[System.Serializable]
public class PlayerBaseAndModifiedStats
{
    [field: SerializeField] public float originalAttack { get; set; }
    [field: SerializeField] public float modifiedAttack { get; set; }

    [field: SerializeField] public float originalDefense { get; set; }
    [field: SerializeField] public float modifiedDefense { get; set; }

    [field: SerializeField] public float modifiedHP { get; set; }

    [field: SerializeField] public float originalSpeed { get; set; }
    [field: SerializeField] public float modifiedSpeed { get; set; }


}
