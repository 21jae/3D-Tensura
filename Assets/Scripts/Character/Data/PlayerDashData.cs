using UnityEngine;

[System.Serializable]
public class PlayerDashData
{

    [field: Header("�뽬 ������")]
    [field: SerializeField] public SOSkill dashSwordSkillData { get; private set; }

    [field: Header("�뽬 ������")]
    [field: SerializeField] public GameObject dashSwordPrefab { get; private set; }
    
    [field: Header("�뽬 ����")]
    [field: SerializeField] [field: Range(3f, 10f)] public float dashDistance { get; private set; } = 5.0f;
    [field: SerializeField] [field: Range(3f, 15f)] public float dashSpeed { get; private set; } = 10.0f;
    [field: SerializeField] [field: Range(3f, 10f)] public float damageRadius { get; private set; } = 8.0f;
    [field: SerializeField] public Vector3 dashTarget { get; private set; }
    [field: SerializeField] public bool isDashing { get; private set; }

    public void SetDashTarget(Vector3 target)
    {
        dashTarget = target;
    } 

    public void SetIsDashing(bool dashing)
    {
        isDashing = dashing;
    }
}

