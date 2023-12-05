using UnityEngine;

public class MonsterFactory : MonoBehaviour, IMonsterFactory    //������ �������� ���� ���� ������Ʈ�� �����ϴ� å���� ���մϴ�.
{
    public GameObject lizardMan;
    public GameObject lizardWoman;

    public GameObject orcBasic;
    public GameObject orcBoss;

    public GameObject wolfBasic;

    public Monster CreateLizard(MonsterTypes.LizardType type, Vector3 position)
    {
        GameObject prefab = null;

        switch (type)
        {
            case MonsterTypes.LizardType.Man:
                prefab = lizardMan;
                break;
            case MonsterTypes.LizardType.Woman:
                prefab = lizardWoman;
                break;
        }

        Monster monster = CreateMonsterPrefab(prefab, position);
        return monster;
    }

    public Monster CreateOrc(MonsterTypes.OrcType type, Vector3 position)
    {
        GameObject prefab = null;

        switch (type)
        {
            case MonsterTypes.OrcType.Basic:
                prefab = orcBasic;
                break;
            case MonsterTypes.OrcType.Boss:
                prefab = orcBoss;
                break;
        }

        Monster monster = CreateMonsterPrefab(prefab, position);
        return monster;
    }

    public Monster CreateWolf(MonsterTypes.WolfType type, Vector3 position)
    {
        GameObject prefab = wolfBasic;

        Monster monster = CreateMonsterPrefab(prefab, position);

        return monster;
    }

    private Monster CreateMonsterPrefab(GameObject prefab, Vector3 position)
    {
        if (prefab)
        {
            GameObject instance = Instantiate(prefab, position, Quaternion.identity);
            return instance.GetComponent<Monster>();
        }
        return null;
    }
}
