using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour   //������ ���͸� ������ ��ġ�� ��ġ�ϰų� �߰����� �ʱ�ȭ �۾��� �����ϴ� ���� å��
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
