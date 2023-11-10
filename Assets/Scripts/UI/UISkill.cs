
using UnityEngine;

public class UISkill : MonoBehaviour
{
    public GameObject PlayerCanvas;     //��ųâ ���� ���� �÷��̾� ĵ����
    public GameObject SkillOnButton;
    public GameObject SkillOffButton;

    private void Start()
    {
        PlayerCanvas.SetActive(false);   
    }

    public void OpenPlayerCanvas()
    {
        PlayerCanvas.SetActive(true);
        SkillOnButton.SetActive(false);
        SkillOffButton.SetActive(true);
    }

    public void ClosePlayerCanvas()
    {
        SkillOnButton.SetActive(true);
        SkillOffButton.SetActive(false);
        PlayerCanvas.SetActive(false);     
    }

}
