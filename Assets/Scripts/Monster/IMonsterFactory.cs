using UnityEngine;

public interface IMonsterFactory
{
    Monster CreateLizard(MonsterTypes.LizardType type, Vector3 position);
    Monster CreateOrc(MonsterTypes.OrcType type, Vector3 position);
    Monster CreateWolf(MonsterTypes.WolfType type, Vector3 position);
}
