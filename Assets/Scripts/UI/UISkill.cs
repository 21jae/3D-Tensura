
using UnityEngine;

public class UISkill : MonoBehaviour
{
    public GameObject PlayerCanvas;     //스킬창 열면 꺼질 플레이어 캔버스
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
