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
        resultText01.text = $"제거된 적의 수: {destoryEnemeyCount} ";
    }
    public void UpdateDealResultText()
    {
        resultText02.text = $"적에게 가한 피해량 : {damageToEnemy} ";
    }
    public void ResultDestoryEnemyCount() => UpdateResultText();
    public void ResultDealToDamageCount() => UpdateDealResultText();

    public void ResultPanelCondition()
    {
        //현재 필드에 TriggerZone01~05까지 존재
        bool triggerZoneExists = GameObject.FindGameObjectsWithTag("TriggerZone").Length > 0;
        bool enemyExists = GameObject.FindGameObjectsWithTag("Enemy").Length > 0;

        // TriggerZone과 Enemy가 모두 사라졌을 경우에만 결과창 활성화
        if (!triggerZoneExists && !enemyExists)
        {
            Debug.Log("On");
            resultPanel.SetActive(true);
        }
    }
}
