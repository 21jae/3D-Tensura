using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHP : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private AnimationCurve hpChangeCurve;
    [SerializeField] private TextMeshProUGUI hpTextMeshPro;
    [SerializeField] private float lerpSpeed = 3f;
    private float targetHpPercentage;

    public SkillManager skillManager;

    void Start()
    {
        UpdateHpUIText();

        targetHpPercentage = playerController.playerStatManager.currentData.currentHP / playerController.playerStatManager.currentData.currentMaxHP;
        slider.value = targetHpPercentage;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            playerController.playerStatManager.currentData.currentHP -= 150f;
        }
        HandleHp();

        slider.value = Mathf.Lerp(slider.value, targetHpPercentage, lerpSpeed * Time.deltaTime);

        UpdateHpUIText();
    }

    private void HandleHp()
    {
        targetHpPercentage = playerController.playerStatManager.currentData.currentHP / playerController.playerStatManager.currentData.currentMaxHP;
    }
    private void UpdateHpUIText()
    {
        hpTextMeshPro.text = string.Format("{0} / {1}", playerController.playerStatManager.currentData.currentHP, playerController.playerStatManager.currentData.currentMaxHP);
    }
}
