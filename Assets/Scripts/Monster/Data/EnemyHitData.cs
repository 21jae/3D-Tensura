using UnityEngine;

[System.Serializable]
public class EnemyHitData
{
    [field: Header("히트 프리팹")]
    [field: SerializeField] public GameObject hitPrefab { get; set; }
    [field: SerializeField] public GameObject deathPrefab {get; set;}


    [field: Header("데미지 변수")]
    [field: SerializeField] public float DamageInterval { get; private set; } = 1.5f;
    [field: SerializeField] public bool isRecoveringFormBigDamage { get; private set; } = false;
    [field: SerializeField] public bool isBeingDestroy { get; private set; } = false;

    public void SetIsBigDamage(bool bigDamage)
    {
        isBeingDestroy = bigDamage;
    }

    public void SetIsBeingDestroy(bool destory) 
    {
        isBeingDestroy = destory;
    }
}