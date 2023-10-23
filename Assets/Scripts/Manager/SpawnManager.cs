using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour   //생성된 몬스터를 적절한 위치에 배치하거나 추가적인 초기화 작업을 수행하는 등의 책임
{
    public MonsterFactory monsterFactory;

    public List<StageInfo> stages = new List<StageInfo>();
    public int currentStage = 0;

    public void SpawnLizard(MonsterTypes.LizardType type, Vector3 position)
    {
        monsterFactory.CreateLizard(type, position);
    }

    public void SpawnOrc(MonsterTypes.OrcType type, Vector3 position)
    {
        monsterFactory.CreateOrc(type, position);
    }

}
