using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public SOSkill skill;
    //public PlayerController player;
    private SkillManager skillManager;
    public Image imageIcon;
    public Image imageCooltime;
    public Text coolTimeText;
    private Button skillButton;

    private void Start()
    {
        skillManager = GetComponent<SkillManager>();
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

        //player.ActivateSkill(skill);
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
