using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private SOSkill skill;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private Image imageIcon;
    [SerializeField] private Image imageCooltime;
    [SerializeField] private Text coolTimeText;
    private Button skillButton;

    private void Start()
    {
        skillButton = GetComponent<Button>();
        imageIcon.sprite = skill.skillIcon;
        imageCooltime.fillAmount = 0;
        coolTimeText.text = "";
    }

    public void OnClicked()
    {
        if (imageCooltime.fillAmount > 0)
            return;

        skillManager.ReadSkill(skill);
        StartCoroutine(SkillCooltime());
    }

    private IEnumerator SkillCooltime()
    {
        skillButton.interactable = false;

        float remainingTime = skill.skillCoolTime;
        imageCooltime.fillAmount = 1;

        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            imageCooltime.fillAmount = remainingTime / skill.skillCoolTime;
            coolTimeText.text = Mathf.CeilToInt(remainingTime).ToString();

            yield return null;
        }

        coolTimeText.text = "";
        imageCooltime.fillAmount = 0;
        skillButton.interactable = true;
    }
}
