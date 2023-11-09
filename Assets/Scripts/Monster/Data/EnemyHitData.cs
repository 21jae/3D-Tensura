using UnityEngine;

[System.Serializable]
public class EnemyHitData
{
    [field: Header("��Ʈ ������")]
    [field: SerializeField] public GameObject hitPrefab { get; set; }
    //[field: SerializeField] public GameObject predationHit {get; set;}
    [field: SerializeField] public GameObject deathPrefab {get; set;}


    [field: Header("������ ����")]
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
//    //������ ������ ����� ����.
//    float damageToTake = amount - characterStatManager.currentData.currentDefense;

//    if (damageToTake < 0f)
//    {
//        damageToTake = 0f;  //���ݷ��� ���º��� ���ٸ� ������ 0
//    }

//    if (Data.GuardData.isGuarding)
//    {
//        damageToTake *= 0.2f;   //�������̶�� �޴� ������ 80%����
//    }

//    characterStatManager.currentData.currentHP -= damageToTake;

//    Debug.Log(characterStatManager.currentData.currentHP);

//    Vector3 monsterWorldPosition = transform.position; // ������ ���� ��ġ
//    UIMonsterHP.Instance.CreateDamagePopup(damageToTake, monsterWorldPosition);

//    if (isPredation && !Data.HitData.hasPredationHitSpawned)    //isPredation(�����ų)�� true�϶� �� hit �������� �����Ѵ�.
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

//    //ū �������� ������ �������� enemy �ִϸ��̼�
//    float damagePercentage = damageToTake / characterStatManager.currentData.currentMaxHP;
//    if (damagePercentage > 0.4f)
//    {
//        StartCoroutine(PlayBigDamageAnimation());
//    }