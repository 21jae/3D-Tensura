using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterHP : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private AnimationCurve hpChangeCurve;
    [SerializeField] private TextMeshProUGUI hpTextMeshPro;
    [SerializeField] private float lerpSpeed = 3f;
    private float targetHpPercentage;

    void Start()
    {
        UpdateHpUIText();

        targetHpPercentage = playerController.playerStatManager.currentHP / playerController.playerStatManager.currentMaxHP;
        slider.value = targetHpPercentage;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            playerController.playerStatManager.currentHP -= 150f;
        }

        HandleHp();

        slider.value = Mathf.Lerp(slider.value, targetHpPercentage, lerpSpeed * Time.deltaTime);

        UpdateHpUIText();
    }

    private void HandleHp()
    {
        targetHpPercentage = playerController.playerStatManager.currentHP / playerController.playerStatManager.currentMaxHP;
    }
    private void UpdateHpUIText()
    {
        hpTextMeshPro.text = string.Format("{0} / {1}", playerController.playerStatManager.currentHP, playerController.playerStatManager.currentMaxHP);
    }
}
