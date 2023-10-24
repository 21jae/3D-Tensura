using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public enum ZoneType
    {
        LizardZone,
        OrcZone
    }

    public ZoneType zoneType;
    public SpawnManager spawnManager;
    public Transform player;

    //보스 소환 구역인지 여부
    public bool isBossZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 bossSpawnPosition;

            switch (zoneType)
            {
                case ZoneType.LizardZone:
                    
                    if (isBossZone)
                    {
                        bossSpawnPosition = transform.position;
                        spawnManager.SpawnLizard(MonsterTypes.LizardType.Boss, bossSpawnPosition);
                    }
                    else
                    {

                        for (int i = 0; i < spawnManager.stages[spawnManager.currentStage].lizardCount; i++)
                        {
                            Vector3 spawnPosition = player.position + new Vector3(Random.Range(-8, 8), 0, Random.Range(-8, 8));
                            int randomLizard = Random.Range(0, 2);  // Lizard Man or Woman 소환
                            MonsterTypes.LizardType type = randomLizard == 0 ? MonsterTypes.LizardType.Man : MonsterTypes.LizardType.Woman;
                            spawnManager.SpawnLizard(type, spawnPosition);
                        }
                    }
                    break;

                case ZoneType.OrcZone:
                    if (isBossZone)
                    {
                        bossSpawnPosition = transform.position;
                        spawnManager.SpawnOrc(MonsterTypes.OrcType.Boss, bossSpawnPosition);
                    }
                    else
                    {
                        for (int i = 0; i < spawnManager.stages[spawnManager.currentStage].orcCount; i++)
                        {
                            Vector3 spawnPosition = player.position + new Vector3(Random.Range(-6, 6), 0, Random.Range(-6, 6));
                            spawnManager.SpawnOrc(MonsterTypes.OrcType.Basic, spawnPosition);
                        }
                    }
                    break;
            }

            Destroy(gameObject);
        }
    }
}
