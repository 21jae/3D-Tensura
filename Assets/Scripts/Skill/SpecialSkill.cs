using UnityEngine;

public class SpecialSkill : MonoBehaviour, ISkill
{
    private PlayerController playerController;
    public GameObject TestPrefab;


    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();

        if (playerController == null )
        {
            Debug.Log("컨트롤러 없음");
        }
    }

    public void ActivateSkill()
    {
        Instantiate(TestPrefab, transform.position, Quaternion.identity);

    }

}
