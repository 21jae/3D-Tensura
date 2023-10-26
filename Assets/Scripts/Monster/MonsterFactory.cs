using UnityEngine;

public class MonsterFactory : MonoBehaviour, IMonsterFactory    //������ �������� ���� ���� ������Ʈ�� �����ϴ� å���� ���մϴ�.
{
    public GameObject lizardMan;
    public GameObject lizardWoman;
    public GameObject lizardBoss;

    public GameObject orcBasic;
    public GameObject orcBoss;

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
            case MonsterTypes.LizardType.Boss:
                prefab = lizardBoss;
                break;
        }

        Monster monster = CreateMonsterPrefab(prefab, position);

        if (type == MonsterTypes.LizardType.Boss)
        {
            monster.SetAsBoss();
            Debug.Log("���ڵ� ���� ���� �� ����");
        }
        else
        {
            Debug.Log("���ڵ� �Ϲ� ���� �� ����");
            monster.SetAsLizard();
        }

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

        Monster monster =  CreateMonsterPrefab(prefab, position);

        if (type == MonsterTypes.OrcType.Boss)
        {
            monster.SetAsBoss();
            Debug.Log("��ũ ���� ���� �� ����");
        }
        else
        {
            monster.SetAsOrc();
            Debug.Log("��ũ �Ϲ� ���� �� ����");
        }

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
