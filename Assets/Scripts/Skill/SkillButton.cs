using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public SOSkill skill;
    public PlayerController player;
    public Image imageIcon;
    public Image imageCooltime;
    public Text coolTimeText;
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
        {
            return;
        }

        player.ActivateSkill(skill);
        StartCoroutine(SkillCooltime());
    }

    private IEnumerator SkillCooltime()
    {
        skillButton.interactable = false;

        float remainingTime = skill.coolTime;
        imageCooltime.fillAmount = 1;

        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            imageCooltime.fillAmount = remainingTime / skill.coolTime;
            coolTimeText.text = Mathf.CeilToInt(remainingTime).ToString();

            yield return null;
        }

        coolTimeText.text = "";
        imageCooltime.fillAmount = 0;
        skillButton.interactable = true;
    }
}
