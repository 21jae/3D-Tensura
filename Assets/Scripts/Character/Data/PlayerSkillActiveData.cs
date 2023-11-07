using UnityEngine;

[System.Serializable]
public class PlayerSkillActiveData
{
    [field: Header("Slime Skills")]
    [field: SerializeField] public bool characterChange = false;

    [field: Header("Human Skills")]
    [field: SerializeField] public bool buffSkill = false;
    [field: SerializeField] public bool dashSkill = false;
    [field: SerializeField] public bool predationSkill = false;
    [field: SerializeField] public bool blessingSkill = false;
    [field: SerializeField] public bool megidoSkill = false;
}
