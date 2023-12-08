using System.Collections;
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
    [SerializeField] private GameObject playerUI;
    [SerializeField] private PlayerController player;

    private int destoryEnemeyCount { get; set; }
    private int damageToEnemy { get; set; }

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
    public void ResultPanelCondition()
    {
        bool triggerZoneExists = GameObject.FindGameObjectsWithTag("TriggerZone").Length > 0;
        bool enemyExists = GameObject.FindGameObjectsWithTag("Enemy").Length > 0;

        if (!triggerZoneExists && !enemyExists)
        {
            //StartCoroutine(PanelAndTimelineDelay());
            SoundManager.Instance.StopBackgroundMusic();
            StartCoroutine(PanelAndTimelineDelay());
        }
    }

    private IEnumerator PanelAndTimelineDelay()
    {
        yield return new WaitForSeconds(0.5f);
        SoundManager.Instance.PlayVictoryBGM();
        playerUI.SetActive(false);
        wing.SetActive(true);
        sword.SetActive(false);
        resultvCam.SetActive(true);

        yield return new WaitForSeconds(3f);
        resultPanel.SetActive(true);
        player.ResultAnimation();
    }
    private void UpdateResultText() => resultText01.text = $"제거된 적의 수: {destoryEnemeyCount} ";
    public void UpdateDealResultText() => resultText02.text = $"적에게 가한 피해량 : {damageToEnemy} ";
    public void ResultDestoryEnemyCount() => UpdateResultText();
    public void ResultDealToDamageCount() => UpdateDealResultText();
    public void SpecialVcam01() => specialCam01.SetActive(true);
    public void SpecialVcam02() => specialCam02.SetActive(true);
    public void SpecialVcamOff()
    {
        specialCam01.SetActive(false);
        specialCam02.SetActive(false);
    }

}
