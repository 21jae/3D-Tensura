using UnityEngine;

public class MonsterFactory : MonoBehaviour, IMonsterFactory    //몬스터의 프리팹을 실제 게임 오브젝트로 생성하는 책임을 지닙니다.
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
            Debug.Log("리자드 보스 적용 및 생성");
        }
        else if (type == MonsterTypes.LizardType.Man)
        {
            Debug.Log("리자드 일반 적용 및 생성");
            monster.SetAsLizard();
        }
        else if (type == MonsterTypes.LizardType.Woman)
        {
            Debug.Log("리자드 일반 적용 및 생성");
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
            Debug.Log("오크 보스 적용 및 생성");
        }
        else
        {
            monster.SetAsOrc();
            Debug.Log("오크 일반 적용 및 생성");
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
