using UnityEngine;

[System.Serializable]
public class EnemyHitData
{
    [field: Header("히트 프리팹")]
    [field: SerializeField] public GameObject hitPrefab { get; set; }
    //[field: SerializeField] public GameObject predationHit {get; set;}
    [field: SerializeField] public GameObject deathPrefab {get; set;}


    [field: Header("데미지 변수")]
    [field: SerializeField] public float DamageInterval { get; private set; } = 1.5f;
    //[field: SerializeField] public bool hasPredationHitSpawned { get; private set; } = false;
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

    //public void SetHasPredation(bool hitPredation)
    //{
    //    hasPredationHitSpawned = hitPredation;
    //}

}
//public void TakeDamage(float amount, bool isPredation = false)
//{
//    //데미지 받을때 수행될 로직.
//    float damageToTake = amount - characterStatManager.currentData.currentDefense;

//    if (damageToTake < 0f)
//    {
//        damageToTake = 0f;  //공격력이 방어력보다 낮다면 데미지 0
//    }

//    if (Data.GuardData.isGuarding)
//    {
//        damageToTake *= 0.2f;   //가드중이라면 받는 데미지 80%감소
//    }

//    characterStatManager.currentData.currentHP -= damageToTake;

//    Debug.Log(characterStatManager.currentData.currentHP);

//    Vector3 monsterWorldPosition = transform.position; // 몬스터의 현재 위치
//    UIMonsterHP.Instance.CreateDamagePopup(damageToTake, monsterWorldPosition);

//    if (isPredation && !Data.HitData.hasPredationHitSpawned)    //isPredation(흡수스킬)이 true일땐 이 hit 프리팹을 생성한다.
//    {
//        //GameObject hitInstance = Instantiate(predationHit, transform.position, Quaternion.identity);
//        //hitInstance.transform.SetParent(transform);
//        Data.HitData.predationHit = ObjectPooling.instance.GetPooledObject("PredationHit");
//        Data.HitData.predationHit.transform.SetParent(transform);
//        Data.HitData.SetHasPredation(true);
//    }
//    else if (!isPredation)
//    {
//        Vector3 effectPosition = transform.position + new Vector3(0f, 1f, 0f);
//        Data.HitData.hitPrefab = ObjectPooling.instance.GetPooledObject("DamageHit");
//        Data.HitData.hitPrefab.transform.position = effectPosition;
//        //Instantiate(hitPrefab, effectPosition, Quaternion.identity);

//    }

//    if (characterStatManager.currentData.currentHP <= 0f)
//    {
//        ChangeState(State.DEATH);
//    }

//    //큰 데미지를 입을시 쓰러지는 enemy 애니메이션
//    float damagePercentage = damageToTake / characterStatManager.currentData.currentMaxHP;
//    if (damagePercentage > 0.4f)
//    {
//        StartCoroutine(PlayBigDamageAnimation());
//    }