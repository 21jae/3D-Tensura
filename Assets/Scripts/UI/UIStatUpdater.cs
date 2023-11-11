using TMPro;
using UnityEngine;

public class UIStatUpdater : MonoBehaviour
{
    [Header("�÷��̾� ����")]
    [SerializeField] private TextMeshProUGUI ATKText;
    [SerializeField] private TextMeshProUGUI DEFText;
    [SerializeField] private TextMeshProUGUI HPText;

    private void Start()
    {
        UpdateStatsUI();
        CharacterStatManager.instance.OnStatsChanged += UpdateStatsUI;
    }

    private void OnDestroy()
    {
        CharacterStatManager.instance.OnStatsChanged -= UpdateStatsUI;
    }

    private void UpdateStatsUI()
    {
        ATKText.text = CharacterStatManager.instance.GetCurrentAttackPower().ToString();
        DEFText.text = CharacterStatManager.instance.GetCurrentDefense().ToString();
        HPText.text = CharacterStatManager.instance.GetCurrentHP().ToString();
    }
}
