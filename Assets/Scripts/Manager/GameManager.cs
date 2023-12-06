using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject gameManager = new GameObject("GameManager");
                    _instance = gameManager.AddComponent<GameManager>();
                }
            }

            return _instance;
        }
    }

    public TMP_Text resultText01;
    public TMP_Text resultText02;
    public GameObject resultPanel;

    private int destoryEnemeyCount;
    private int damageToEnemy;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        resultPanel.SetActive(false);
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        UpdateResultText();
        UpdateDealResultText();
        ResultPanelCondition();
    }

    public void IncreaseDestroyedEnemyCount()
    {
        destoryEnemeyCount++;
        UpdateResultText();
    }
    public void RecordDamageToEnemy(int damage)
    {
        damageToEnemy += damage;
    }

    private void UpdateResultText()
    {
        resultText01.text = $"���ŵ� ���� ��: {destoryEnemeyCount} ";
    }
    public void UpdateDealResultText()
    {
        resultText02.text = $"������ ���� ���ط� : {damageToEnemy} ";
    }
    public void ResultDestoryEnemyCount() => UpdateResultText();
    public void ResultDealToDamageCount() => UpdateDealResultText();

    public void ResultPanelCondition()
    {
        //���� �ʵ忡 TriggerZone01~05���� ����
        bool triggerZoneExists = GameObject.FindGameObjectsWithTag("TriggerZone").Length > 0;
        bool enemyExists = GameObject.FindGameObjectsWithTag("Enemy").Length > 0;

        // TriggerZone�� Enemy�� ��� ������� ��쿡�� ���â Ȱ��ȭ
        if (!triggerZoneExists && !enemyExists)
        {
            Debug.Log("On");
            resultPanel.SetActive(true);
        }
    }
}
