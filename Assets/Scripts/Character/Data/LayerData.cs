using UnityEngine;

[System.Serializable]
public class LayerData
{
    [field: SerializeField] public LayerMask GroundLayer { get; private set; }
}