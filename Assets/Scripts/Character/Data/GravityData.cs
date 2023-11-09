using UnityEngine;

[System.Serializable]
public class GravityData
{
    [field: SerializeField] public float gravity { get; private set; } = -9.81f;
    [field: SerializeField] public float VerticalVelocity { get; set; }
    [field: SerializeField] public bool isGravityEnabled { get; private set; } = false;
    
    public void SetIsGravity(bool _gravity)
    {
        isGravityEnabled = _gravity;
    }

}
