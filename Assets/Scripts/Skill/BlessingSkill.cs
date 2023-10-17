using System;
using System.Collections;
using UnityEngine;

public class BlessingSkill : MonoBehaviour
{
    public GameObject blessingHuman;
    public GameObject blessingEffect;
    public GameObject buffEffect;
    private float offsetDistance = 2f;
    private float destoryPrefab = 4f;

    /// <summary>
    /// �������ͽ� ���
    /// </summary>
    private PlayerController playerController;
    private float healthBuff = 0.3f;    //30%����
    private float defenseBuff = 0.2f;   //20%����
    private float attackPowerBuff = 0.15f;   //15%����

    public void ActivateBless()
    {
        Vector3 spawnPosition = transform.position - transform.forward * offsetDistance;
        Quaternion rotationTowardsPlayer = Quaternion.LookRotation(transform.position - spawnPosition);

        GameObject bless = Instantiate(blessingHuman, spawnPosition, rotationTowardsPlayer);

        StartCoroutine(WaitAndBless(bless, destoryPrefab));
    }

    private IEnumerator WaitAndBless(GameObject blessing, float seconds)
    {
        yield return new WaitForSeconds(seconds);

        Destroy(blessing);
        BuffToPlayer();
    }

    private void BuffToPlayer()
    {
        Instantiate(blessingEffect, transform.position, Quaternion.identity);
        Debug.Log("ü�� �� �������ͽ� ���!");

        GameObject buff = Instantiate(buffEffect, transform.position, Quaternion.identity);
        buff.transform.SetParent(transform);
    }
}
