using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;

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

    [SerializeField] private TMP_Text resultText01;
    [SerializeField] private TMP_Text resultText02;
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private GameObject resultvCam;
    [SerializeField] private GameObject specialCam01;
    [SerializeField] private GameObject specialCam02;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject wing;
    [SerializeField] private PlayerController player;
    
    private int destoryEnemeyCount      { get; set; }
    private int damageToEnemy           { get; set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        InitGameObject();
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void InitGameObject()
    {
        resultPanel.SetActive(false);
        resultvCam.SetActive(false);
        specialCam01.SetActive(false);
        specialCam02.SetActive(false);
    }

    private void Update()
    {
        UpdateResultText();
        UpdateDealResultText();
        ResultPanelCondition();

        if (Input.GetKeyDown(KeyCode.G))
            StartCoroutine(PanelAndTimelineDelay());
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
            StartCoroutine(PanelAndTimelineDelay());
        }
    }

    private IEnumerator PanelAndTimelineDelay()
    {
        SoundManager.Instance.StopMusic();
        yield return new WaitForSeconds(0.25f);
        SoundManager.Instance.PlayVictoryBGM();
        wing.SetActive(false);
        sword.SetActive(false);
        resultvCam.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        player.ResultAnimation();
        resultPanel.SetActive(true);
    }

    public void SpecialVcam01() => specialCam01.SetActive(true);
    public void SpecialVcam02() => specialCam02.SetActive(true);
    public void SpecialVcamOff()
    {
        specialCam01.SetActive(false);
        specialCam02.SetActive(false);
    }

}
